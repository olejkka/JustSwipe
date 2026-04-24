using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Character Spawn Chances",
        fileName = "Character Spawn Chances"
    )]
    public class CharacterSpawnChancesConfig : ScriptableObject
    {
        public float SpawnChanceOneCharacter = 0.35f;
        public float SpawnChanceTwoCharacter = 0.1f;
    }
}