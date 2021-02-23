using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI {
    public class UIShopCharacterSlot : MonoBehaviour {
        public Action<UIShopCharacterSlot> SlotSelected;

        [SerializeField] private TextMeshProUGUI text = null;
        [SerializeField] private Image background = null;

        public string Name => text.text;

        public void Initialize(string name) {
            text.text = name;
            background.color = GetColorFromName(name);
        }

        public void Purchase() {
            SlotSelected?.Invoke(this);
        }

        private Color GetColorFromName(string name) {
            switch (name) {
                // Tier 1
                case "Mage":
                    return new Color32(123, 34, 154, 255);
                case "Warrior":
                    return new Color32(200, 255, 255, 255);
                case "Priest":
                    return new Color32(150, 255, 255, 255);
                case "Hunter":
                    return new Color32(100, 255, 255, 255);
                case "Paladin":
                    return new Color32(50, 255, 255, 255);
                case "Rogue":
                    return new Color32(0, 255, 255, 255);
                case "Fighter":
                    return new Color32(255, 200, 255, 255);
                case "Ranger":
                    return new Color32(255, 150, 255, 255);
                case "Cleric":
                    return new Color32(255, 100, 255, 255);
                case "Bard":
                    return new Color32(255, 50, 255, 255);
                case "Summoner":
                    return new Color32(255, 0, 255, 255);
                case "Tank":
                    return new Color32(255, 255, 200, 255);
                case "Bladecaller":
                    return new Color32(255, 255, 150, 255);
                // Tier 2
                case "Cultist":
                    return new Color32(255, 255, 100, 255);
                case "Assassin":
                    return new Color32(255, 255, 50, 255);
                case "Druid":
                    return new Color32(255, 255, 0, 255);
                case "Healer":
                    return new Color32(200, 200, 200, 255);
                case "Beastmaster":
                    return new Color32(150, 200, 200, 255);
                case "Wizard":
                    return new Color32(100, 200, 200, 255);
                case "Sorcerer":
                    return new Color32(50, 200, 200, 255);
                case "Berserker":
                    return new Color32(0, 200, 200, 255);
                case "Knight":
                    return new Color32(200, 150, 200, 255);
                case "Archon":
                    return new Color32(200, 100, 200, 255);
                case "Herald":
                    return new Color32(200, 50, 200, 255);
                case "Pirate":
                    return new Color32(200, 00, 200, 255);
                case "Necromancer":
                    return new Color32(200, 200, 150, 255);
                // Tier 3
                case "Enchanter":
                    return new Color32(200, 200, 100, 255);
                case "Sage":
                    return new Color32(200, 200, 50, 255);
                case "Warlock":
                    return new Color32(200, 200, 0, 255);
                case "Monk":
                    return new Color32(150, 150, 150, 255);
                case "Templar":
                    return new Color32(100, 150, 150, 255);
                case "Sentinel":
                    return new Color32(50, 150, 150, 255);
                case "Battlemage":
                    return new Color32(0, 150, 150, 255);
                case "Protector":
                    return new Color32(150, 100, 150, 255);
                case "Mystic":
                    return new Color32(150, 50, 150, 255);
                case "Elementalist":
                    return new Color32(150, 0, 150, 255);
                case "Conjurer":
                    return new Color32(150, 150, 100, 255);
                case "Arbiter":
                    return new Color32(150, 150, 50, 255);
                case "Shaman":
                    return new Color32(150, 150, 0, 255);
                // Tier 4
                case "Seer":
                    return new Color32(200, 150, 150, 255);
                case "Revenant":
                    return new Color32(255, 150, 150, 255);
                case "Trickster":
                    return new Color32(150, 200, 150, 255);
                case "Provoker":
                    return new Color32(150, 255, 150, 255);
                case "Keeper":
                    return new Color32(150, 150, 200, 255);
                case "Invoker":
                    return new Color32(150, 150, 250, 255);
                case "Wanderer":
                    return new Color32(255, 200, 200, 255);
                case "Siren":
                    return new Color32(200, 255, 200, 255);
                case "Crusader":
                    return new Color32(200, 200, 255, 255);
                case "Reaper":
                    return new Color32(100, 100, 100, 255);
                case "Broodwarden":
                    return new Color32(50, 100, 100, 255);
                // Tier 5
                case "Dreadnought":
                    return new Color32(0, 100, 100, 255);
                case "Stalker":
                    return new Color32(100, 50, 100, 255);
                case "Illusionist":
                    return new Color32(100, 0, 100, 255);
                case "Strider":
                    return new Color32(100, 100, 50, 255);
                case "Betrayer":
                    return new Color32(100, 100, 0, 255);
                case "Naturalist":
                    return new Color32(150, 100, 100, 255);
                case "Charlatan":
                    return new Color32(100, 150, 100, 255);
                case "Vindicator":
                    return new Color32(100, 100, 150, 255);
                default:
                    return new Color32(200, 100, 100, 255);
            }
        }
    }
}
