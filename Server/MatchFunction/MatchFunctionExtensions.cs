using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenMatch {
    public partial class Program {
        private class MatchFunctionExtensions {
            public static async Task<RepeatedField<Ticket>> QueryPoolAsync(ServerCallContext context, QueryService.QueryServiceClient queryService, Pool pool) {
                var query = queryService.QueryTickets(new QueryTicketsRequest() { Pool = pool }, null, null, context.CancellationToken);
                if (query == null) {
                    return null;
                }

                var tickets = new RepeatedField<Ticket>();
                try {
                    while (await query.ResponseStream.MoveNext()) {
                        tickets.Add(query.ResponseStream.Current.Tickets);
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    query.Dispose();
                    return null;
                } finally { if (query != null) query.Dispose(); }

                return tickets;
            }

            public static async Task<Dictionary<string, RepeatedField<Ticket>>> QueryPoolsAsync(ServerCallContext context, QueryService.QueryServiceClient queryService, RepeatedField<Pool> pools) {
                var results = new List<Result>();
                foreach (var pool in pools) {
                    var result = new Result() {
                        name = pool.Name
                    };
                    var tickets = await QueryPoolAsync(context, queryService, pool);
                    if (tickets == null || tickets.Count <= 0) {
                        continue;
                    }

                    result.tickets = tickets;
                    results.Add(result);

                    if (context.CancellationToken.IsCancellationRequested) {
                        Console.WriteLine("Context cancelled while querying pools");
                        break;
                    }
                }

                var poolMap = new Dictionary<string, RepeatedField<Ticket>>();
                foreach (var result in results) {
                    if (context.CancellationToken.IsCancellationRequested) {
                        Console.WriteLine("Context cancelled while querying pools");
                        return null;
                    }

                    poolMap[result.name] = result.tickets;
                }
                return poolMap;
            }

            private struct Result {
                public RepeatedField<Ticket> tickets { get; set; }
                public string name { get; set; }
            }
        }
    }
}
