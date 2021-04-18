using UnityEngine;

namespace Client.UI {
    public class UILeaveGame : MonoBehaviour {
        public void LeaveGame() {
            Manager.GameClient.Disconnect();
        }
    }
}
