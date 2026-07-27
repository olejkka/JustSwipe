using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Initial Gameplay",
        fileName = "Initial Gameplay"
    )]
    public class InitialGameplayConfig : ScriptableObject
    {
        [SerializeField] private string _playerCharacter;
        [SerializeField] private string _botCharacter;
        [SerializeField] private int _maxPlayerCharactersCount;
        [SerializeField] private int _maxEffectsCount;
        [SerializeField] private int _moneyCount;
        
        public string PlayerCharacter => _playerCharacter;
        public string BotCharacter => _botCharacter;
        public int MaxPlayerCharactersCount => _maxPlayerCharactersCount;
        public int MaxEffectsCount => _maxEffectsCount;
        public int MoneyCount => _moneyCount;
    }
}