using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public BotPhase RequireListedPhase(string definitionId)
        {
            var matches = 0;
            var phase = default(BotPhase);

            if (_defaultBots.Contains(definitionId))
            {
                matches++;
                phase = BotPhase.Default;
            }

            if (_hardBots.Contains(definitionId))
            {
                matches++;
                phase = BotPhase.Hard;
            }

            if (_bossBots.Contains(definitionId))
            {
                matches++;
                phase = BotPhase.Boss;
            }

            if (matches != 1)
                throw new InvalidOperationException($"Definition '{definitionId}' must be in exactly one bot phase list, found {matches}.");

            return phase;
        }

        public string GetRandomDefaultBot() => GetRandom(_defaultBots);
        public string GetRandomHardBot() => GetRandom(_hardBots);
        public string GetRandomBoss() => GetRandom(_bossBots);

        private static string GetRandom(IReadOnlyList<string> bots) =>
            bots[Random.Range(0, bots.Count)];
    }
}
