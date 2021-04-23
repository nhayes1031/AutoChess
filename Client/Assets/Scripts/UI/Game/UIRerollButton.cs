using UnityEngine;

namespace Client.UI {
    public class UIRerollButton : MonoBehaviour {
        public void Reroll() {
            Manager.GameClient.SendRerollRequest();
        }    
    }
}
