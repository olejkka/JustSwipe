using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.Economy
{
    public class DeathRewardService
    {
        private readonly PlayerMoney _playerMoney;

        
        public DeathRewardService(PlayerMoney playerMoney)
        {
            _playerMoney = playerMoney;
        }

        public void GiveReward(Character character)
        {
            var reward = character.CharacterConfig.Reward;
            
            if (reward > 0)
                _playerMoney.AddMoney(reward);
            
            Debug.Log($"Character{character}, Reward {reward}");
        }
    }
}