using UnityEngine;
using Client;
using Client.Game;
using TMPro;

public class UITransitionDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI transitionText = null;

    private void Start() {
        StaticManager.GameClient.TransitionUpdate += DisplayTransitionUpdate;
        StaticManager.GameClient.TransitionUpdate += ReadTransitionPacket;
    }

    private void DisplayTransitionUpdate(TransitionUpdatePacket packet) {
        transitionText.text = packet.Event;
    }

    private void ReadTransitionPacket(TransitionUpdatePacket packet) {
        
    }
}
