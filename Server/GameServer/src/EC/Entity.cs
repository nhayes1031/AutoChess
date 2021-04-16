using Server.Game.Systems;
using System;
using System.Linq;

namespace Server.Game.EC {
    public class Entity : IEntity {
        public Guid entityId { private set; get; }
        public Entity parent { set; get; }
        public bool toBeDeleted { set; get; }

        // Allows for only 10 components to be added to a single gameobject
        protected Component[] bag;
        protected Entity[] children;

        public Entity(params Component[] bag) {
            entityId = Guid.NewGuid();

            this.bag = new Component[10];
            children = new Entity[256];

            if (bag.Length > 10)
                throw new Exception("Tried to initialize an entity with too many components");

            for (int i = 0; i < bag.Length; i++) {
                this.bag[i] = bag[i];
                this.bag[i].SetParent(this);
            }
        }

        public void Update(double time, double deltaTime, Board board) {
            foreach (var item in bag) {
                item?.Update(time, deltaTime, board);
            }
        }

        public T GetComponent<T>() where T: Component {
            foreach (var item in bag) {
                if (item is T)
                    return (T)item;
            }
            return default;
        }

        public T[] GetComponents<T>() where T: Component {
            return bag.Where(item => item is T)
                .Select(item => (T)item)
                .ToArray();
        }

        public T GetComponentInChildren<T>() where T: Component {
            foreach(var child in children) {
                if (child != null) {
                    var component = child.GetComponentInChildren<T>();
                    if (component != null) {
                        return component;
                    }

                    component = GetComponent<T>();
                    if (component != null) {
                        return component;
                    }
                }
            }
            return default;
        }

        public T AddComponent<T>() where T: Component, new() {
            Component comp = new T();
            comp.SetParent(this);
            for (int i = 0; i < bag.Length; i++) {
                if (bag[i] == null) {
                    bag[i] = comp;
                    return comp as T;
                }
            }
            throw new Exception("More than 10 components were tried to be added to a single entity!");
        }

        public void Destroy(Entity entity) {
            for (int i = 0; i < children.Length; i++) {
                if (children[i].entityId == entity.entityId) {
                    children[i].toBeDeleted = true;
                    return;
                }
            }
            throw new Exception("Tried to delete an entity that wasn't present");
        }

        public void Destroy(Component component) {
            for (int i = 0; i < bag.Length; i++) {
                if (bag[i].componentId == component.componentId) {
                    bag[i].toBeDeleted = true;
                    return;
                }
            }
            throw new Exception("Tried to delete a component that wasn't present");
        }

        public Entity Add(Entity entity) {
            for (int i = 0; i < children.Length; i++) {
                if (children[i] == null) {
                    children[i] = entity;
                    children[i].parent = this;
                    return entity;
                }
            }
            throw new Exception("Too many children where added to one entity");
        }

        public static bool operator ==(Entity a, Entity b) => !(a is null) && !(b is null) && a.Equals(b);
        public static bool operator !=(Entity a, Entity b) => !(a is null) && !(b is null) && !a.Equals(b);
        public override bool Equals(object obj) => obj is Entity entity && Equals(entity);
        public bool Equals(Entity entity) => entityId == entity.entityId;
        public bool Equals(Entity a, Entity b) => a.Equals(b);
        public override string ToString() => $"{entityId}";
        public override int GetHashCode() => entityId.GetHashCode();
    }
}
