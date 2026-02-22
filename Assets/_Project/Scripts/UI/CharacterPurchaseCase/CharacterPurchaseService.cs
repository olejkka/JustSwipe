using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Wallet;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseService
    {
        private readonly CharacterCreator _characterCreator;
        private readonly Money _money;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly CharactersStorage _charactersStorage;

        
        public CharacterPurchaseService(
            CharacterCreator characterCreator, 
            Money money,
            InitialGameplayConfig initialGameplayConfig,
            CharactersStorage charactersStorage
            )
        {
            _characterCreator = characterCreator;
            _money = money;
            _initialGameplayConfig = initialGameplayConfig;
            _charactersStorage = charactersStorage;
        }

        public bool CanPurchase(int price)
        {
            return _money.Amount >= price && 
                   _charactersStorage.GetCharactersByTeam(Team.Player).Count() < 
                   _initialGameplayConfig.MaxPlayerCharactersCount;
        }

        public bool TryPurchase(CharacterType characterType, int price)
        {
            if (!CanPurchase(price))
                return false;

            _money.ChangeAmount(-price);
            _characterCreator.CreateOnRandomPos(characterType);
            return true;
        }
    }
}