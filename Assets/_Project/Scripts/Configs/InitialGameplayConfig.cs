using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Initial Gameplay",
        fileName = "Initial Gameplay"
    )]
    public class InitialGameplayConfig : ScriptableObject
    {
        public int MaxPlayerCharactersCount;
        public int MoneyCount;
        public CharacterType PlayerCharacter;
        public CharacterType BotCharacter;
    }
}