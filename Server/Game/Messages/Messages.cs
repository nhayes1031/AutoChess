using Lidgren.Network;
using Server.Game.EC;
using Server.Game.Systems;
using System.Collections.Generic;

namespace Server.Game.Messages {
    public struct PurchaseRerollRequested {
        public NetConnection client;
    }
    public struct PurchaseUnitRequested {
        public NetConnection client;
        public PurchaseUnitPacket packet;
    }
    public struct MoveToBoardFromBenchRequested {
        public NetConnection client;
        public MoveToBoardFromBenchPacket packet;
    }
    public struct MoveToBenchFromBoardRequested {
        public NetConnection client;
        public MoveToBenchFromBoardPacket packet;
    }
    public struct RepositionOnBoardRequested {
        public NetConnection client;
        public RepositionOnBoardPacket packet;
    }
    public struct RepositionOnBenchRequested {
        public NetConnection client;
        public RepositionOnBenchPacket packet;
    }
    public struct SellUnitFromBenchRequested {
        public NetConnection client;
        public SellUnitFromBenchPacket packet;
    }
    public struct SellUnitFromBoardRequested {
        public NetConnection client;
        public SellUnitFromBoardPacket packet;
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
        public PlayerData bottom;
        public PlayerData top;
        public List<Entity> units;
    }
    public struct SimulationEndedInVictory {
        public NetConnection winner;
        public NetConnection loser;
    }
    public struct SimulationEndedInDraw {
        public NetConnection participant1;
        public NetConnection participant2;
    }
    public struct SimulationUnitDied {
        public List<NetConnection> connections;
        public HexCoords unit;
    }
}
