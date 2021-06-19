namespace Client.Game {
    public enum OutgoingPacketTypes {
        Connect,
        Disconnect,
        RequestMoveUnit,
        RequestReroll,
        RequestUnitPurchase,
        RequestXPPurchase,
        RequestUnitSell,
    }

    public enum IncomingPacketTypes {
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
