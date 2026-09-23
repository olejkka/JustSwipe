using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Bot Phase Characters",
        fileName = "Bot Phase Characters"
    )]
    public class BotPhaseCharactersConfig : ScriptableObject
    {
        [SerializeField] private List<string> _defaultBots = new();
        [SerializeField] private List<string> _hardBots = new();
        [SerializeField] private List<string> _bossBots = new();
        [SerializeField] private int _botHardPhaseThreshold;
        [SerializeField] private int _botBossPhaseThreshold;

        public int BotHardPhaseThreshold => _botHardPhaseThreshold;
        public int BotBossPhaseThreshold => _botBossPhaseThreshold;

        public string GetRandomDefaultBot() => GetRandom(_defaultBots);
        public string GetRandomHardBot() => GetRandom(_hardBots);

        private static string GetRandom(IReadOnlyList<string> bots) =>
            bots[Random.Range(0, bots.Count)];
    }
}
