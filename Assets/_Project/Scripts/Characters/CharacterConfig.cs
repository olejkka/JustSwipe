using UnityEngine;

namespace _Project.Scripts.Characters
{
    [CreateAssetMenu(
        menuName = "Gameplay/CharacterConfig",
        fileName = "CharacterConfig"
    )]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Team Team { get; private set; }
        [field: SerializeField] public int Reward { get; private set; }
        [field: SerializeField] public Stats BaseStats { get; private set; }
    }

    [System.Serializable]
    public struct Stats
    {
        public int Health;
        public int Attack;
    }
}