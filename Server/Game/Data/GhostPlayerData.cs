using Lidgren.Network;
using Server.Game.Systems;
using System;
using System.Collections.Generic;

namespace Server.Game {
    public class GhostPlayerData : IPlayer {
        public Guid Id { get; private set; }
        public NetConnection Connection { get; private set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }
        public int XP { get; set; }
        public FixedSizeList<CharacterData> Bench { get; set; }
        public Dictionary<HexCoords, CharacterData> Board { get; set; }
        public FixedSizeList<CharacterData> Shop { get; set; }
        
        public GhostPlayerData(IPlayer playerData) {
            Id = playerData.Id;
            Connection = playerData.Connection;
            Health = playerData.Health;
            Level = playerData.Level;
            Gold = playerData.Gold;
            XP = playerData.XP;
            Bench = playerData.Bench;
            Board = playerData.Board;
            Shop = playerData.Shop;
        }
    }
}
