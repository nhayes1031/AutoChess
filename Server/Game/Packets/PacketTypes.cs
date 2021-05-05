namespace Server.Game {
    public enum IncomingPacketTypes {
        Connect,
        Disconnect,
        RequestMoveUnit,
        RequestReroll,
        RequestUnitPurchase,
        RequestXPPurchase,
        RequestUnitSell,
    }

    public enum OutgoingPacketTypes {
        Connect,
        Disconnect,
        InitialGameSetup,
        StateTransition,
        UpdatePlayer,
        UnitRepositioned,
        RerollPurchased,
        UnitPurchased,
        UnitLeveledUp,
        UnitSold,
        XPPurchased,
        PlayerDied,
        GameOver,
        CombatStarted,
        UnitAttacked,
        UnitMoved,
        UnitDied,
        CombatEndedInDraw,
        CombatEndedInVictory,
        CombatEndedInLoss
    }
}

