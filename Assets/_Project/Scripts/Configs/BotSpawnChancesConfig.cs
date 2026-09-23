using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Character Spawn Chances",
        fileName = "Character Spawn Chances"
    )]
    public class BotSpawnChancesConfig : ScriptableObject
    {
        [Header("Chances")]
        [SerializeField] private float _spawnChanceOneCharacter = 0.35f;
        [SerializeField] private float _spawnChanceTwoCharacters = 0.1f;
        
        public float SpawnChanceOneCharacter => _spawnChanceOneCharacter;
        public float SpawnChanceTwoCharacters => _spawnChanceTwoCharacters;
    }
}