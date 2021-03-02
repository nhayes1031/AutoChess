using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIShop : MonoBehaviour {
        [SerializeField] private HorizontalLayoutGroup grid = null;
        [SerializeField] private GameObject characterPrefab = null;

        private UIShopCharacterSlot[] characterSlots;
        private bool canPurchase = true;

        private void Start() {
            StaticManager.GameClient.ShopUpdate += HandleShopUpdate;
            StaticManager.GameClient.UnitPurchased += HandleUnitPurchased;

            FindObjectOfType<UIBench>().BenchFull += HandleFullBench;

            characterSlots = new UIShopCharacterSlot[5] {
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>(),
                Instantiate(characterPrefab, grid.transform).GetComponent<UIShopCharacterSlot>()
            };

            foreach (var slot in characterSlots) {
                slot.SlotSelected += HandleSlotSelected;
            }
        }

        private void OnApplicationQuit() {
            StaticManager.GameClient.ShopUpdate -= HandleShopUpdate;
            StaticManager.GameClient.UnitPurchased -= HandleUnitPurchased;

            FindObjectOfType<UIBench>().BenchFull -= HandleFullBench;
        }

        private void HandleSlotSelected(UIShopCharacterSlot slot) {
            Debug.Log(canPurchase);
            if (canPurchase) {
                StaticManager.GameClient.Purchase(slot.Name);
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
