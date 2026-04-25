using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Character Spawn Chances",
        fileName = "Character Spawn Chances"
    )]
    public class BotSpawnChancesConfig : ScriptableObject
    {
        [Header("Bot Definitions")]
        [SerializeField] private List<string> _defaultBots = new();
        [SerializeField] private List<string> _secondaryBots = new();
        
        [Header("Chances")]
        [SerializeField] private float _spawnChanceOneCharacter = 0.35f;
        [SerializeField] private float _spawnChanceTwoCharacters = 0.1f;
        
        public float SpawnChanceOneCharacter => _spawnChanceOneCharacter;
        public float SpawnChanceTwoCharacters => _spawnChanceTwoCharacters;
        
        
        public string GetRandomDefaultBot() => GetRandomBot(_defaultBots);
        public string GetRandomSecondaryBot() => GetRandomBot(_secondaryBots);
        
        private string GetRandomBot(IReadOnlyList<string> bots)
        {
            if (bots == null || bots.Count == 0)
            {
                Debug.LogError($"{nameof(BotSpawnChancesConfig)} has empty bot list", this);
                return null;
            }
            return bots[Random.Range(0, bots.Count)];
        }
    }
}