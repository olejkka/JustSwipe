using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay/InitialGameplayConfig",
        fileName = "InitialGameplayConfig"
    )]
    public class InitialGameplayConfig : ScriptableObject
    {
        public int MaxPlayerCharactersCount;
        public int MoneyCount;
        public CharacterType PlayerCharacter;
        public CharacterType BotCharacter;
    }
}