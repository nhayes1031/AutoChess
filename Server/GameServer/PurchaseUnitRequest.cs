using Lidgren.Network;

namespace Server.Game {
    public class PurchaseUnitRequest {
        public string Name { get; set; }
        public NetConnection connection { get; set; }
    }
}
