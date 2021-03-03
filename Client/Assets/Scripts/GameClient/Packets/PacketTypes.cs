﻿namespace Client.Game {
    public enum PacketTypes {
        Connect,
        Disconnect,
        InitialGameSetup,
        TransitionUpdate,
        UpdatePlayerInfo,
        PurchaseUnit,
        PurchaseXP,
        PurchaseReroll,
        MoveToBoardFromBench,
        RepositionOnBoard,
        MoveToBenchFromBoard,
        RepositionOnBench,
        SellUnitFromBench,
        SellUnitFromBoard
    }
}
