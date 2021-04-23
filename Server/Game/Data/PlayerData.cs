using Lidgren.Network;
using PubSub;
using Server.Game.Messages;
using Server.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Game {
    public class PlayerData : IPlayer {
        public static Action<PlayerData> PlayerCreated;

        private const int XP_COST = 4;
        private const int REROLL_COST = 2;

        public Guid Id { private set; get; }
        public NetConnection Connection { private set; get; }
        private int health;
        public int Health { 
            get { return health; }
            set {
                health = value;
                if (health <= 0) {
                    Hub.Default.Publish(new PlayerDied() {
                        who = Id
                    });
                }
            } 
        }
        public int Level { get; set; }
        public int Gold { get; set; }
        public int XP { get; set; }
        public FixedSizeList<CharacterData> Bench { get; set; }
        public Dictionary<HexCoords, CharacterData> Board { get; set; }
        public FixedSizeList<CharacterData> Shop { get; set; }

        public bool IsAlive => Health > 0;

        public PlayerData(NetConnection connection) {
            this.Connection = connection;
            Id = Guid.NewGuid();
            Health = 1;
            Level = 1;
            Gold = 0;
            XP = 0;

            Shop = new FixedSizeList<CharacterData>(5);
            Bench = new FixedSizeList<CharacterData>(10);
            Board = new Dictionary<HexCoords, CharacterData>();

            PlayerCreated?.Invoke(this);
        }

        public void AddReward(Reward reward) {
            Gold += reward.Gold;
            XP += reward.XP;
        }

        public void UpdateShop(CharacterData[] newShop) {
            if (newShop.Length == 5) {
                Shop.Clear();
                Shop.AddRange(newShop);
            } else {
                Logger.Error("Tried to assign a new shop that did not contain 5 elements.");
            }
        }

        public IEnumerable<string> GetShopAsStringArray() {
            return Shop.List.Select(x => {
                if (!(x is null)) {
                    return x.Name;
                }
                return null;
            });
        }

        public bool Purchase(string name) {
            var character = CharacterFactory.CreateFromName(name);
            if (!(character is null)) {
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
                    XP -= Level * XP_COST;
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

        public bool SellUnit(CharacterData character, HexCoords coords) {
            if (!AllowedCoordinates.Contains(coords))
                return false;

            var unit = Board[coords];
            if (!(unit is null)) {
                Board.Remove(coords);
                Gold += character.Cost;
                return true;
            }
            return false;
        }

        public bool SellUnit(CharacterData character, int seat) {
            if (seat < Bench.Size && (Bench.List[seat] is null)) {
                Bench.List[seat] = null;
                Gold += character.Cost;
                return true;
            }
            return false;
        }

        public bool MoveUnit(CharacterData character, HexCoords fromCoords, int toSeat) {
            if (!AllowedCoordinates.Contains(fromCoords))
                return false;

            var unit = Board[fromCoords];
            if (!(unit is null) 
                && Bench.Size < toSeat && Bench.List[toSeat] is null
            ) {
                Bench.List[toSeat] = unit;
                Board.Remove(fromCoords);
                return true;
            }
            return false;
        }

        public bool MoveUnit(CharacterData character, HexCoords fromCoords, HexCoords toCoords) {
            if (!AllowedCoordinates.Contains(fromCoords)
                && !AllowedCoordinates.Contains(toCoords)   
            )
                return false;

            var unit = Board[fromCoords];
            if (!(unit is null)
                && AllowedCoordinates.Contains(toCoords) && !Board.ContainsKey(toCoords)
            ) {
                Board[toCoords] = unit;
                Board.Remove(fromCoords);
                return true;
            }
            return false;
        }

        public bool MoveUnit(CharacterData character, int fromSeat, HexCoords toCoords) {
            if (!AllowedCoordinates.Contains(toCoords))
                return false;

            if (fromSeat >= 0 && fromSeat < Bench.List.Length && Bench.List[fromSeat] == character
                && AllowedCoordinates.Contains(toCoords) && !Board.ContainsKey(toCoords)
            ) {
                var unit = Bench.List[fromSeat];
                Bench.List[fromSeat] = null;
                Board[toCoords] = unit;
                return true;
            }
            return false;
        }

        public bool MoveUnit(CharacterData character, int fromSeat, int toSeat) {
            if (Bench.List.Length < fromSeat && Bench.List.Length < toSeat
                && Bench.List[fromSeat] == character && Bench.List[toSeat] is null
            ) {
                Bench.List[toSeat] = Bench.List[fromSeat];
                Bench.List[fromSeat] = null;
                return true;
            }
            return false;
        }

        private readonly HashSet<HexCoords> AllowedCoordinates = new() {
            new HexCoords(0, 0),
            new HexCoords(1, 0),
            new HexCoords(2, 0),
            new HexCoords(3, 0),
            new HexCoords(4, 0),
            new HexCoords(5, 0),
            new HexCoords(6, 0),
            new HexCoords(7, 0),
            new HexCoords(0, 1),
            new HexCoords(1, 1),
            new HexCoords(2, 1),
            new HexCoords(3, 1),
            new HexCoords(4, 1),
            new HexCoords(5, 1),
            new HexCoords(6, 1),
            new HexCoords(7, 1),
            new HexCoords(-1, 2),
            new HexCoords(0, 2),
            new HexCoords(1, 2),
            new HexCoords(2, 2),
            new HexCoords(3, 2),
            new HexCoords(4, 2),
            new HexCoords(5, 2),
            new HexCoords(6, 2),
            new HexCoords(-1, 3),
            new HexCoords(0, 3),
            new HexCoords(1, 3),
            new HexCoords(2, 3),
            new HexCoords(3, 3),
            new HexCoords(4, 3),
            new HexCoords(5, 3),
            new HexCoords(6, 3)
        };
    }
}
