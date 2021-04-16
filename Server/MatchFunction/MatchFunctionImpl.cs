using Google.Protobuf.Collections;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenMatch {
    public partial class Program {
        private class MatchFunctionImpl : MatchFunction.MatchFunctionBase {
            private QueryService.QueryServiceClient queryService;

            public MatchFunctionImpl(QueryService.QueryServiceClient queryService) {
                this.queryService = queryService;
            }

            public override async Task Run(RunRequest request, IServerStreamWriter<RunResponse> responseStream, ServerCallContext context) {
                try {
                    Console.WriteLine("Generating proposals for function " + request.Profile.Name);

                    var poolTickets = await MatchFunctionExtensions.QueryPoolsAsync(context, queryService, request.Profile.Pools);
                    if (poolTickets == null) {
                        Console.WriteLine("Failed to query tickets for the given pools");
                        return;
                    }

                    var proposals = MakeMatches(request.Profile, poolTickets);
                    if (proposals == null) {
                        Console.WriteLine("Failed to generate matches");
                        return;
                    }

                    Console.WriteLine("Streaming " + proposals.Count + " proposals to Open Match");
                    foreach (var proposal in proposals) {
                        var status = responseStream.WriteAsync(new RunResponse() { Proposal = proposal });
                        if (status == null) {
                            Console.WriteLine("Failed to stream proposals to Open Match");
                            return;
                        }
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
                return;
            }

            private static List<Match> MakeMatches(MatchProfile profile, Dictionary<string, RepeatedField<Ticket>> poolTickets) {
                var matches = new List<Match>();
                var count = 0;

                // Loop through all ticket pools
                // Create a match for each group of tickets
                foreach (var entry in poolTickets) {
                    if (entry.Value.Count < TICKETS_PER_POOL_PER_MATCH) {
                        break;
                    }

                    var tickets = new RepeatedField<Ticket>();
                    // loop over all tickets and create matches for each one
                    foreach (var ticket in entry.Value) {
                        tickets.Add(ticket);
                        if (tickets.Count == TICKETS_PER_POOL_PER_MATCH) {
                            // create a match
                            var match = new Match() {
                                MatchId = "profile-" + profile.Name + "-time-" + DateTime.UtcNow + "-" + count,
                                MatchProfile = profile.Name,
                                MatchFunction = MATCH_NAME
                            };
                            match.Tickets.AddRange(tickets);
                            matches.Add(match);

                            tickets.Clear();
                            count++;
                        }
                    }
                }
                
                return matches;
            }
        }
    }
}
