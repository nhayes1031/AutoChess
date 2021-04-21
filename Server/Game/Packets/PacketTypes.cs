namespace Server.Game {
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
        SellUnitFromBoard,
        SimulationUnitMoved,
        SimulationUnitAttacked,
        SimulationEndedInDraw,
        SimulationEndedInVictory,
        SimulationEndedInLoss,
        SimulationCombatStarted,
        SimulationUnitDied,
        PlayerDied,
        GameOver
    }
}

