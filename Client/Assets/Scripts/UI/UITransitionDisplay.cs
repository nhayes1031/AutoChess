using UnityEngine;
using Client;
using Client.Game;
using TMPro;

public class UITransitionDisplay : MonoBehaviour {
    [SerializeField] private  TextMeshProUGUI transitionText = null;

    private void Start() {
        Manager.GameClient.TransitionUpdate += DisplayTransitionUpdate;
        Manager.GameClient.TransitionUpdate += ReadTransitionPacket;
    }

    private void DisplayTransitionUpdate(TransitionUpdatePacket packet) {
        transitionText.text = packet.Event;
    }

    private void ReadTransitionPacket(TransitionUpdatePacket packet) {
        
    }
}
