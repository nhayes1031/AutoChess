using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace OpenMatch {
    public partial class Program {
        private const string QUERY_SERVICE_ADDRESS = "http://my-release-open-match-query.open-match.svc.cluster.local:50503";
        private const int SERVER_PORT = 50502;
        private const string MATCH_NAME = "basic-matchfunction";
        private const int TICKETS_PER_POOL_PER_MATCH = 2;

        public static async Task Main(string[] args) {
            using (var channel = GrpcChannel.ForAddress(QUERY_SERVICE_ADDRESS)) {
                var queryService = new QueryService.QueryServiceClient(channel);
                Console.WriteLine("Query service created");
                var service = new MatchFunctionImpl(queryService);
                Console.WriteLine("MatchFunctionImpl created");

                Server server = null;
                try {
                    server = new Server() {
                        Services = { MatchFunction.BindService(service) },
                        Ports = { new ServerPort("0.0.0.0", SERVER_PORT, ServerCredentials.Insecure) }
                    };

                    Console.WriteLine("Grpc server created");
                    server.Start();
                    Console.WriteLine("Grpc server started");
                    await server.ShutdownTask;
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    return;
                } finally {
                    if (server != null) {
                        server.ShutdownAsync().Wait();
                    }
                }
            }
        }
    }
}
