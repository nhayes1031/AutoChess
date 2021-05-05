using Lidgren.Network;
using Server.Game.EC;
using Server.Game.Systems;
using System;
using System.Collections.Generic;

namespace Server.Game.Messages {
    public struct PurchaseRerollRequested {
        public NetConnection client;
    }
    public struct PurchaseUnitRequested {
        public NetConnection client;
        public RequestUnitPurchasePacket packet;
    }
    public struct MoveUnitRequested {
        public NetConnection client;
        public RequestMoveUnitPacket packet;
    }
    public struct SellUnitRequested {
        public NetConnection client;
        public RequestUnitSellPacket packet;
    }
    public struct PurchaseXPRequested {
        public NetConnection client;
    }
    public struct LockMessageHandler {
        public bool status;
    }
    public struct GameFinished { }
    public struct SimulationUnitMoved {
        public List<NetConnection> connections;
        public HexCoords fromCoords;
        public HexCoords toCoords;
    }
    public struct SimulationUnitAttacked {
        public List<NetConnection> connections;
        public HexCoords attacker;
        public HexCoords defender;
        public int damage;
    }
    public struct SimulationCombatStarted {
        public IPlayer bottom;
        public IPlayer top;
        public List<Tuple<string, BoardLocation>> units;
    }
    public struct SimulationEndedInVictory {
        public NetConnection winner;
        public NetConnection loser;
    }
    public struct SimulationEndedInDraw {
        public NetConnection participant1;
        public int participant1Damage;
        public NetConnection participant2;
        public int participant2Damage;
    }
    public struct SimulationUnitDied {
        public List<NetConnection> connections;
        public HexCoords unit;
    }
    public struct PlayerDied {
        public Guid who;
    }
    public struct GameOver {
        public Guid Winner;
    }
    public struct UnitPurchased {
        public NetConnection connection;
        public int shopIndex;
    }
    public struct UnitLeveledUp {
        public NetConnection connection;
        public List<Tuple<string, ILocation>> units;
        public string name;
        public ILocation location;
        public int starLevel;
    }
}
