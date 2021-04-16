using Server.Game.Systems;
using System;

namespace Server.Game.EC {
    public interface IEntity {
        Guid entityId { get; }
        Entity parent { get; set; }
        bool toBeDeleted { get; set; }
        void Update(double time, double deltaTime, Board board);
    }
}
