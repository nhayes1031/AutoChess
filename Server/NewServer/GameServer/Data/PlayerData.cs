using System.Collections.Generic;
using System.Linq;

namespace Server.Game {
    // TODO: Test this bitch
    public class PlayerData {
        private const int XP_COST = 4;
        private const int REROLL_COST = 2;

        public int Level;
        public int Gold;
        public int XP;
        public CharacterList Bench;
        public Board Board;
        public CharacterList Shop;

        public PlayerData() {
            Level = 1;
            Gold = 0;
            XP = 0;

            Shop = new CharacterList(5);
            Bench = new CharacterList(10);
            Board = new Board();
        }

        public void AddReward(Reward reward) {
            Gold += reward.Gold;
            XP += reward.XP;
        }

        public void UpdateShop(Character[] newShop) {
            if (newShop.Length == 5) {
                Shop.Clear();
                Shop.AddRange(newShop);
            } else {
                Logger.Error("Tried to assign a new shop that did not contain 5 elements.");
            }
        }

        public IEnumerable<string> GetShopAsStringArray() {
            return Shop.List.Select(x => {
                if (x != null) {
                    return x.Name;
                }
                return null;
            });
        }

        public bool Purchase(string name) {
            var character = CharacterFactory.CreateFromName(name);
            if (character != null) {
                if (Shop.Contains(character)) {
                    if (Gold >= 0) {
                        Gold -= 0;
                        
                        Shop.Remove(character);
                        Bench.Add(character);
                        
                        return true;
                    } else {
                        Logger.Error("Someone tried to request a character that they didn't have enough gold for!");
                    }
                } else {
                    Logger.Error("Someone tried to request a character that wasn't in their shop!");
                }
            } else {
                Logger.Error("Someone tried to request a character that didn't exist!");
            }
            return false;
        }

        public void PurchaseXP() {
            if (Level >= 10)
                return;

            // TODO: Maybe this should be handled by an XP table. 
            if (Gold >= 0) {
                Gold -= 0;
                XP += XP_COST;

                // Handle the case when we have leveled up.
                if (XP >= Level * XP_COST) {
                    XP = XP - Level * XP_COST;
                    Level++;
                }
            }
        }

        public bool PayForReroll() {
            if (Gold >= 0) {
                Gold -= 0;
                return true;
            }
            return false;
        }

        public bool SellUnit(Character character, HexCoords coords) {
            if (Board.Contains(character, coords)) {
                Board.RemoveUnit(coords);
                Gold += character.Cost;
                return true;
            }
            return false;
        }

        public bool SellUnit(Character character, int seat) {
            if (Bench.Size < seat && Bench.List[seat] != null) {
                Bench.List[seat] = null;
                Gold += character.Cost;
                return true;
            }
            return false;
        }

        public bool MoveUnit(Character character, HexCoords fromCoords, int toSeat) {
            if (Board.Contains(character, fromCoords) 
                && Bench.Size < toSeat && Bench.List[toSeat] == null
            ) {
                var unit = Board.RemoveUnit(fromCoords);
                Bench.List[toSeat] = unit;
                return true;
            }
            return false;
        }

        public bool MoveUnit(Character character, HexCoords fromCoords, HexCoords toCoords) {
            if (Board.Contains(character, fromCoords)
                && Board.Contains(toCoords) && Board.IsHexEmpty(toCoords)
            ) {
                Board.MoveUnit(fromCoords, toCoords);
                return true;
            }
            return false;
        }

        public bool MoveUnit(Character character, int fromSeat, HexCoords toCoords) {
            if (Bench.List.Length < fromSeat && Bench.List[fromSeat] == character
                && Board.Contains(toCoords) && Board.IsHexEmpty(toCoords)
            ) {
                var unit = Bench.List[fromSeat];
                Bench.List[fromSeat] = null;
                Board.AddUnit(unit, toCoords);
                return true;
            }
            return false;
        }

        public bool MoveUnit(Character character, int fromSeat, int toSeat) {
            if (Bench.List.Length < fromSeat && Bench.List.Length < toSeat
                && Bench.List[fromSeat] == character && Bench.List[toSeat] == null
            ) {
                Bench.List[toSeat] = Bench.List[fromSeat];
                Bench.List[fromSeat] = null;
                return true;
            }
            return false;
        }
    }
}
