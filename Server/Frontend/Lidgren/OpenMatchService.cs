using Grpc.Core;
using Grpc.Net.Client;
using Lidgren.Network;
using OpenMatch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend {
    public class OpenMatchService {
        private const string OM_FRONTEND_ENDPOINT_IN_CLUSTER = "http://my-release-open-match-frontend.open-match.svc.cluster.local:50504";

        public Action<NetConnection, Assignment> connectionAssigned;

        private Dictionary<NetConnection, Ticket> queuedTickets;
        private FrontendService.FrontendServiceClient client;

        public OpenMatchService() {
            var channel = GrpcChannel.ForAddress(OM_FRONTEND_ENDPOINT_IN_CLUSTER);
            client = new FrontendService.FrontendServiceClient(channel);

            queuedTickets = new Dictionary<NetConnection, Ticket>();
        }

        public async Task<Ticket> CreateTicket(NetConnection connection) {
            // verify connection isn't in queue
            if (queuedTickets.ContainsKey(connection)) {
                // You can't queue for two matches, silly!
                return null;
            }

            // generate ticket
            var request = MakeTicket();
            var ticket = await client.CreateTicketAsync(request);
            if (ticket == null) {
                Logger.Error($"[{connection.RemoteEndPoint}]: Failed to create ticket");
                return null;
            }

            // monitor ticket assignment
            new Task(async () => await WatchAssignment(connection, ticket)).Start();

            Logger.Info($"[{connection}]: Ticket created successfully, id: {ticket.Id}");
            queuedTickets[connection] = ticket;
            return ticket;
        }

        public async Task<bool> DeleteTicket(NetConnection connection) {
            if (queuedTickets.ContainsKey(connection)) {
                var ticket = queuedTickets[connection];
                var request = new DeleteTicketRequest() { TicketId = ticket.Id };
                
                await client.DeleteTicketAsync(request);
                queuedTickets.Remove(connection);

                return true;
            }

            return false;
        }

        private async Task WatchAssignment(NetConnection connection, Ticket ticket) {
            try {
                var stream = client.WatchAssignments(new WatchAssignmentsRequest() { TicketId = ticket.Id });
                var assignment = new Assignment();
                while (assignment.Connection == "") {
                    // Chill a little before checking again
                    Thread.Sleep(100);

                    while (await stream.ResponseStream.MoveNext()) {
                        if (stream.ResponseStream.Current != null) {
                            assignment = stream.ResponseStream.Current.Assignment;
                            connectionAssigned?.Invoke(connection, assignment);
                            queuedTickets.Remove(connection);
                            return;
                        }
                    }
                }
            }
            catch (Exception) {
                // Ticket was removed. Verify that it isn't in the queuedTickets
                if (queuedTickets.ContainsKey(connection)) {
                    Logger.Error("A ticket was removed from the queue without being removed from the queuedTickets");
                }
            }
        }

        private static CreateTicketRequest MakeTicket() {
            var mode = "mode.standard";

            var sf = new SearchFields();
            sf.Tags.Add(mode);

            return new CreateTicketRequest() {
                Ticket = new Ticket() {
                    SearchFields = sf
                }
            };
        }
    }
}