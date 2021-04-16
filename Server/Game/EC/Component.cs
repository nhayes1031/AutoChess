using Server.Game.Systems;
using System;

namespace Server.Game.EC {
    public abstract class Component : IComponent {
        public Guid componentId { private set; get; }
        public Entity parent { private set; get; }
        public bool toBeDeleted { set; get; }

        public Component() {
            componentId = Guid.NewGuid();
        }

        public virtual void Update(double time, double deltaTime, Board board) { }

        public void SetParent(Entity parent) {
            this.parent = parent;
        }
    }
}
