using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/BotsDeathRewardsConfig",
        fileName = "BotsDeathRewardsConfig"
    )]
    public class BotsDeathRewardsConfig : ScriptableObject
    {
        public List<BotDeathRewardEntry> BotDeathRewardEntries = new();
        
        [Serializable]
        public class BotDeathRewardEntry
        {
            public string Id;
            public int Reward;
        }
    }
}