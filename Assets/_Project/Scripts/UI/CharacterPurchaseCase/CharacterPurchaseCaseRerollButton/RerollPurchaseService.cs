using _Project.Scripts.GameplayEconomy;

namespace _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton
{
    public class RerollPurchaseService
    {
        private readonly GameplayMoney _gameplayMoney;


        public RerollPurchaseService(GameplayMoney gameplayMoney)
        {
            _gameplayMoney = gameplayMoney;
        }
        
        private bool CanPurchase(int price)
        {
            return _gameplayMoney.Amount >= price;
        }
        
        public bool TryPurchase(int price)
        {
            if (!CanPurchase(price))
                return false;

            _gameplayMoney.ChangeAmount(-price);
            return true;
        }
    }
}