using Client.Game;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIBench : MonoBehaviour {
        public Action<bool> BenchFull;
        public Action<int, string> SeatSelected;

        [SerializeField] private HorizontalLayoutGroup grid = null;
        [SerializeField] private GameObject characterPrefab = null;

        private UIBenchSlot[] benchSlots;

        // TODO: Maybe this should be reading from the bench on UpdatePlayerInfo
        private void Start() {
            StaticManager.GameClient.UnitPurchased += HandleUnitPurchased;
            StaticManager.GameClient.UnitSold += HandleUnitSold;

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

            for (int i = 0; i < benchSlots.Length; ++i) {
                benchSlots[i].gameObject.SetActive(false);
                benchSlots[i].SetSeat(i);
                benchSlots[i].SeatSelected += HandleSeatSelected;
            }
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.UnitPurchased -= HandleUnitPurchased;
        }

        private void HandleSeatSelected(int seat) {
            SeatSelected?.Invoke(seat, benchSlots[seat].Unit);
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

        private void HandleUnitSold(SellUnitFromBenchPacket packet) {
            benchSlots[packet.Seat].gameObject.SetActive(false);
            benchSlots[packet.Seat].Clear();
            BenchFull?.Invoke(false);
        }
    }
}
