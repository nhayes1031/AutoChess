namespace Server.Game.Systems {
    public static class RewardSystem {
        public static Reward GetRewardsFor(PlayerData player) {
            return new Reward() {
                XP = Constants.XP_PER_ROUND,
                Gold = CalculateGoldReward(player.Level, player.Gold)
            };
        }

        private static int CalculateGoldReward(int level, int ownedGold) {
            var interest = ((ownedGold % (10 * 10)) - (ownedGold % 10)) / 10;
            var total = interest + level;
            return total;
        }
    }
}
