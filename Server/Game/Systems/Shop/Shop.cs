namespace Server.Game.Systems {
    public class Shop {
        private readonly CharacterPool pool;

        public Shop() {
            pool = new CharacterPool();
        }

        public Breed[] RequestReroll(int level, Breed[] previousShop) {
            pool.Push(previousShop);
            return pool.Pop(5, level);
        }
    }
}
