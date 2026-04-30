using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.GameplayEconomy;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseService
    {
        private readonly CharacterCreator _characterCreator;
        private readonly GameplayMoney _gameplayMoney;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly CharactersStorage _charactersStorage;

        
        public CharacterPurchaseService(
            CharacterCreator characterCreator, 
            GameplayMoney gameplayMoney,
            InitialGameplayConfig initialGameplayConfig,
            CharactersStorage charactersStorage
            )
        {
            _characterCreator = characterCreator;
            _gameplayMoney = gameplayMoney;
            _initialGameplayConfig = initialGameplayConfig;
            _charactersStorage = charactersStorage;
        }

        public bool TryPurchase(string definitionId, int price)
        {
            if (!CanPurchase(price))
                return false;

            _gameplayMoney.ChangeAmount(-price);
            _characterCreator.CreateOnRandomPos(definitionId);
            return true;
        }
        
        private bool CanPurchase(int price)
        {
            return _gameplayMoney.Amount >= price && 
                   _charactersStorage.GetCharactersByTeam(Team.Player).Count() < 
                   _initialGameplayConfig.MaxPlayerCharactersCount;
        }
    }
}