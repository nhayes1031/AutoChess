namespace Server.Game.Systems {
    public class Shop {
        private CharacterPool pool;

        public Shop() {
            pool = new CharacterPool();
        }

        public CharacterData[] RequestReroll(PlayerData player) {
            var characters = new CharacterData[5];
            for (int i = 0; i < 5; i++) {
                characters[i] = pool.GetCharacter(player.Level);
            }
            return characters;
        }
    }
}
