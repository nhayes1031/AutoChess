using UnityEngine;
using Client;
using Client.Game;
using TMPro;

public class UILevelDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI levelText;

    void Start() {
        StaticManager.GameClient.UpdatePlayerInfo += DisplayPlayerLevel;
    }

    private void DisplayPlayerLevel(UpdatePlayerInfoPacket packet) {
        levelText.text = "Level: " + packet.Level;
    }
}