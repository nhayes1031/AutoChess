using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIShop : MonoBehaviour {
        [SerializeField] private  HorizontalLayoutGroup grid = null;
        [SerializeField] private  GameObject characterPrefab = null;

        private UIShopCharacterSlot[] characterSlots;
        private bool canPurchase = true;

        private void Start() {
            Manager.GameClient.ShopUpdate += HandleShopUpdate;
            Manager.GameClient.UnitPurchased += HandleUnitPurchased;

            FindObjectOfType<UIBench>().BenchFull += HandleFullBench;

            characterSlots = new UIShopCharacterSlot[5] {
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>()
            };

            for (int i = 0; i < characterSlots.Length; i++) {
                characterSlots[i].Index = i;
                characterSlots[i].SlotSelected += HandleSlotSelected;
            }
        }

        private void OnApplicationQuit() {
            Manager.GameClient.ShopUpdate -= HandleShopUpdate;
            Manager.GameClient.UnitPurchased -= HandleUnitPurchased;

            FindObjectOfType<UIBench>().BenchFull -= HandleFullBench;
        }

        private void HandleSlotSelected(UIShopCharacterSlot slot) {
            if (canPurchase) {
                Manager.GameClient.Purchase(slot.Name, slot.Index);
            }
        }

        private void HandleFullBench(bool status) {
            canPurchase = !status;
        }

        private void HandleShopUpdate(string[] shop) {
            if (characterSlots.Length == shop.Length) {
                for (int i = 0; i < shop.Length; i++) {
                    if (shop[i] != "") {
                        characterSlots[i].gameObject.SetActive(true);
                        characterSlots[i].Initialize(shop[i]);
                    }
                }
            } else {
                Debug.LogError("Shop tried to update with too many elements");
            }
        }

        private void HandleUnitPurchased(string name) {
            foreach (var slot in characterSlots) {
                if (slot.Name == name) {
                    slot.gameObject.SetActive(false);
                    slot.Clear();
                    return;
                }
            }
        }
    }
}
