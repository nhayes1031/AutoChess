using Lidgren.Network;
using Server.Game.Systems;
using System;
using System.Collections.Generic;

namespace Server.Game {
    public interface IPlayer {
        Guid Id { get; }
        NetConnection Connection { get; }
        int Health { get; set; }
        int Level { get; set; }
        int Gold { get; set; }
        int XP { get; set; }
        FixedSizeList<StarEntity> Bench { get; set; }
        Dictionary<HexCoords, StarEntity> Board { get; set; }
        Breed[] Shop { get; set; }
    }
}
