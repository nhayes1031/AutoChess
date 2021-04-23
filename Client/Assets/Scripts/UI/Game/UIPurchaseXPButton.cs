using UnityEngine;

namespace Client.UI {
    public class UIPurchaseXPButton : MonoBehaviour {
        public void PurchaseXP() {
            Manager.GameClient.SendPurchaseXPRequest();
        }
    }
}
