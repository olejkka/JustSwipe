using System.Linq;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Economy
{
    public class BotDeathRewardService
    {
        private readonly BotsDeathRewardsContig _botsDeathRewardsContig;
        private readonly PlayerMoney _playerMoney;

        public BotDeathRewardService(BotsDeathRewardsContig botsDeathRewardsContig, PlayerMoney playerMoney)
        {
            _botsDeathRewardsContig = botsDeathRewardsContig;
            _playerMoney = playerMoney;
        }

        public void ProcessBotDeath(string botId)
        {
            var reward = GetRewardForBot(botId);
            if (reward > 0)
            {
                _playerMoney.ChangeMoney(reward);
            }
        }

        private int GetRewardForBot(string botId)
        {
            var rewardEntry = _botsDeathRewardsContig.BotDeathRewardEntries
                .FirstOrDefault(entry => entry.Id == botId);

            return rewardEntry?.Reward ?? 0;
        }
    }
}