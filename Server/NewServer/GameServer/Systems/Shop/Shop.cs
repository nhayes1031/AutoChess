namespace Server.Game {
    public class Shop {
        private CharacterPool pool;

        public Shop() {
            pool = new CharacterPool();
        }

        public Character[] RequestReroll(PlayerData player) {
            var characters = new Character[5];
            for (int i = 0; i < 5; i++) {
                characters[i] = pool.GetCharacter(player.Level);
            }
            return characters;
        }
    }
}
