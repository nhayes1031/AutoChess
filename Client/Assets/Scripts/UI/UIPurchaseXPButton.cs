using UnityEngine;

namespace Client.UI {
    public class UIPurchaseXPButton : MonoBehaviour {
        public void PurchaseXP() {
            StaticManager.GameClient.SendPurchaseXPRequest();
        }
    }
}
