using Grpc.Core;
using Grpc.Net.Client;
using OpenMatch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Director {
    public class Program {
        private const string OM_BACKEND_ENDPOINT = "http://localhost:50505";
        private const string MATCH_FUNCTION_HOST_NAME = "127.0.0.1";
        private const int MATCH_FUNCTION_PORT = 50502;

        private static BackendService.BackendServiceClient backendServiceClient;
        private static Random rnd;

        public static async Task Main(string[] args) {
            rnd = new Random();

            using (var channel = GrpcChannel.ForAddress(OM_BACKEND_ENDPOINT)) {
                Console.WriteLine("Connected to Open Match Backend");
                backendServiceClient = new BackendService.BackendServiceClient(channel);

                var profile = GenerateProfile();
                Console.WriteLine($"Fetching matches for {profile.Name}");
                while (true) {
                    // Chill a little before rechecking for matches
                    Thread.Sleep(500);
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

                // TODO: Add Agones connection
                var conn = $"{rnd.Next(0, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}.{rnd.Next(0, 255)}:2222";
                var assignments = new AssignmentGroup() {
                    Assignment = new Assignment() {
                        Connection = conn
                    }
                };
                assignments.TicketIds.AddRange(ticketIds);

                var request = new AssignTicketsRequest();
                request.Assignments.Add(assignments);

                var status = await backendServiceClient.AssignTicketsAsync(request);
                if (status == null) {
                    Console.WriteLine("AssignTickets failed for match " + match.MatchId);
                    return false;
                }

                Console.WriteLine("Assigned server " + conn + " to match " + match.MatchId);
                return true;
            }
            Console.WriteLine("No matches to assign");
            return false;
        }
    }
}
