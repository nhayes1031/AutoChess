using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIBench : MonoBehaviour {
        public Action<bool> BenchFull;

        [SerializeField] private HorizontalLayoutGroup grid = null;
        [SerializeField] private GameObject characterPrefab = null;

        private UIBenchSlot[] benchSlots;

        private void Start() {
            StaticManager.GameClient.UnitPurchased += HandleUnitPurchased;

            benchSlots = new UIBenchSlot[10] {
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIBenchSlot>()
            };

            foreach (var slot in benchSlots) {
                slot.gameObject.SetActive(false);
            }
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.UnitPurchased -= HandleUnitPurchased;
        }

        private void HandleUnitPurchased(string name) {
            foreach (var slot in benchSlots) {
                if (!slot.HasUnit) {
                    slot.gameObject.SetActive(true);
                    slot.AddUnit(name);
                    CheckForFullBench();
                    return;
                }
            }
        }

        private void CheckForFullBench() {
            foreach (var slot in benchSlots) {
                if (!slot.HasUnit) {
                    BenchFull?.Invoke(false);
                    return;
                }
            }
            BenchFull?.Invoke(true);
        }
    }
}
