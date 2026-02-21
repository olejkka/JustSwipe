using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;
using _Project.Scripts.Wallet;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseService
    {
        private readonly CharacterCreator _characterCreator;
        private readonly Money _money;

        
        public CharacterPurchaseService(CharacterCreator characterCreator, Money money)
        {
            _characterCreator = characterCreator;
            _money = money;
        }

        public bool CanPurchase(int price) => _money.Amount >= price;

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