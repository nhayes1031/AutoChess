using System.Collections;
using TMPro;
using UnityEngine;

namespace Client.UI {
    public class UIRoundResult : MonoBehaviour {
        [SerializeField] private  TextMeshProUGUI textField = null;

        private void Start() {
            textField.gameObject.SetActive(false);

            Manager.GameClient.SimulationEndedInVictory += HandleVictory;
            Manager.GameClient.SimulationEndedInLoss += HandleLoss;
            Manager.GameClient.SimulationEndedInDraw += HandleDraw;
        }

        private void OnDestroy() {
            Manager.GameClient.SimulationEndedInVictory -= HandleVictory;
            Manager.GameClient.SimulationEndedInLoss -= HandleLoss;
            Manager.GameClient.SimulationEndedInDraw -= HandleDraw;
        }

        private void HandleVictory() {
            DisplayText("Victory");
        }
        
        private void HandleLoss() {
            DisplayText("Loss");
        }

        private void HandleDraw() {
            DisplayText("Draw");
        }

        private void DisplayText(string value) {
            textField.gameObject.SetActive(true);
            textField.text = value;

            StartCoroutine(HideText());
        }

        private IEnumerator HideText() {
            yield return new WaitForSeconds(0.5f);
            textField.gameObject.SetActive(false);
        }
    }
}
