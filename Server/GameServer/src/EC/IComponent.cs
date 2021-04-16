using Server.Game.Systems;
using System;

namespace Server.Game.EC {
    public interface IComponent {
        Guid componentId { get; }
        Entity parent { get; }
        bool toBeDeleted { get; set; }
        void Update(double time, double deltaTime, Board board);
    }
}
