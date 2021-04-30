using Allocation;
using Grpc.Core;
using Grpc.Net.Client;
using OpenMatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Director {
    public class Program {
        private const string OM_BACKEND_ENDPOINT = "http://my-release-open-match-backend.open-match.svc.cluster.local:50505";
        private const string AGONES_ALLOCATION_SERVICE_ENDPOINT = "35.230.73.227:443";
        private const string MATCH_FUNCTION_HOST_NAME = "matchfunction.autochess.svc.cluster.local";
        private const int MATCH_FUNCTION_PORT = 50502;

        private static BackendService.BackendServiceClient backendServiceClient;
        private static AllocationService.AllocationServiceClient agonesClient;

        public static async Task Main(string[] args) {
            using var c1 = GrpcChannel.ForAddress(OM_BACKEND_ENDPOINT);
            backendServiceClient = new BackendService.BackendServiceClient(c1);
            Console.WriteLine("Connected to Open Match Backend");

            var cacert = File.ReadAllText(@"./ca.crt");
            var clientcert = File.ReadAllText(@"./client.crt");
            var clientkey = File.ReadAllText(@"./client.key");
            var credentials = new SslCredentials(cacert, new KeyCertificatePair(clientcert, clientkey));

            var channel = new Channel(
                AGONES_ALLOCATION_SERVICE_ENDPOINT, 
                credentials, 
                new List<ChannelOption> { 
                    new ChannelOption(
                        "grpc.service_config",
                        "{\"loadBalancingConfig\":[{\"grpclb\":{}}]}"
                    ) 
                }
            );
            agonesClient = new AllocationService.AllocationServiceClient(channel);
            Console.WriteLine("Connected to Agones Allocator");

            var profile = GenerateProfile();
            Console.WriteLine($"Fetching matches for {profile.Name}");
            while (true) {
                // Chill a little before rechecking for matches
                Thread.Sleep(5000);
                var matches = await FetchAsync(profile);
                if (matches == null || matches.Count <= 0) {
                    Console.WriteLine("Failed to fetch matches for profile " + profile.Name);
                    continue;
                }

                Console.WriteLine("Generated " + matches.Count + " matches for profile " + profile.Name);
                if (!await AssignAsync(matches)) {
                    Console.WriteLine("Failed to assign servers to matches");
                    continue;
                }
            }
        }

        public static MatchProfile GenerateProfile() {
            var mode = "mode.standard";
            var tagPresentFilter = new TagPresentFilter() { Tag = mode };

            var pool = new Pool() { Name = "pool_mode_" + mode };
            pool.TagPresentFilters.Add(tagPresentFilter);

            var profile = new MatchProfile() { Name = "mode_based_profile" };
            profile.Pools.Add(pool);

            return profile;
        }

        public static async Task<List<Match>> FetchAsync(MatchProfile profile) {
            var request = new FetchMatchesRequest() {
                Config = new FunctionConfig() {
                    Host = MATCH_FUNCTION_HOST_NAME,
                    Port = MATCH_FUNCTION_PORT,
                    Type = FunctionConfig.Types.Type.Grpc
                },
                Profile = profile
            };

            var stream = backendServiceClient.FetchMatches(request);
            if (stream == null) {
                return null;
            }

            var results = new List<Match>();
            try {
                while (await stream.ResponseStream.MoveNext()) {
                    results.Add(stream.ResponseStream.Current.Match);
                }
            } catch (Exception) {
                stream.Dispose();
                return results;
            } finally { if (stream != null) stream.Dispose(); }

            return results;
        }

        public static async Task<bool> AssignAsync(List<Match> matches) {
            foreach (var match in matches) {
                var ticketIds = new List<string>();
                foreach (var ticket in match.Tickets) {
                    ticketIds.Add(ticket.Id);
                }

                try {
                    Console.WriteLine("Calling Agones");
                    var response = await agonesClient.AllocateAsync(
                        new AllocationRequest {
                            Namespace = "default"
                        }
                    );
                    Console.WriteLine(response);
                    Console.WriteLine("Constructed response");

                    var conn = $"{response.Address}:{response.Ports[0].Port}";
                    var assignments = new AssignmentGroup() {
                        Assignment = new Assignment() {
                            Connection = conn
                        }
                    };
                    assignments.TicketIds.AddRange(ticketIds);

                    Console.WriteLine("Constructed assignments");

                    var request = new AssignTicketsRequest();
                    request.Assignments.Add(assignments);

                    Console.WriteLine("Constructed response");

                    var status = await backendServiceClient.AssignTicketsAsync(request);
                    if (status == null) {
                        Console.WriteLine("AssignTickets failed for match " + match.MatchId);
                        return false;
                    }

                    Console.WriteLine("Assigned server " + conn + " to match " + match.MatchId);
                    return true;
                } catch (RpcException e) {
                    Console.WriteLine($"gRPC error: {e}");
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("No matches to assign");
            return false;
        }
    }
}
