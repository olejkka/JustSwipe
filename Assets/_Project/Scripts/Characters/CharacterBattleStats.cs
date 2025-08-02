using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharacterBattleStats : MonoBehaviour
    {
        public string Id { get; private set; }
        public Team Team { get; private set; }
        public int MaxHealth { get; private set; }
        public int AttackDamage { get; private set; }

        public Vector2Int Position { get; private set; }
        

        public void Init(CharacterStatsConfig.CharacterStatsEntry stats, Vector2Int spawnPosition)
        {
            Id = stats.Id;
            Team = stats.team;
            MaxHealth = stats.BaseMaxHealth;
            AttackDamage = stats.BaseAttackDamage;
            Position = spawnPosition;
        }
    }
}