using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.UI.Shop;

namespace _Project.Scripts.GameplayEconomy
{
    public class ShopPurchaseService
    {
        private readonly CharacterCreator _characterCreator;
        private readonly GameplayMoney _gameplayMoney;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly CharactersStorage _charactersStorage;
        private readonly EventBus _eventBus;

        
        public ShopPurchaseService(
            CharacterCreator characterCreator,
            GameplayMoney gameplayMoney,
            InitialGameplayConfig initialGameplayConfig,
            CharactersStorage charactersStorage,
            EventBus eventBus)
        {
            _characterCreator = characterCreator;
            _gameplayMoney = gameplayMoney;
            _initialGameplayConfig = initialGameplayConfig;
            _charactersStorage = charactersStorage;
            _eventBus = eventBus;
        }

        public bool TryPurchase(ShopOffer offer)
        {
            return offer.Type switch
            {
                ShopOfferType.Character => TryPurchaseCharacter(offer.DefinitionId, offer.Price),
                ShopOfferType.Effect => TryPurchaseEffect(offer.DefinitionId, offer.Price, Team.Player),
                _ => false
            };
        }

        private bool TryPurchaseCharacter(string definitionId, int price)
        {
            if (!CanPurchaseCharacter(price))
                return false;

            _gameplayMoney.ChangeAmount(-price);
            _characterCreator.CreateOnRandomPos(definitionId);
            return true;
        }

        private bool TryPurchaseEffect(string definitionId, int price, Team team)
        {
            if (!CanPurchaseEffect(price, team))
                return false;

            _gameplayMoney.ChangeAmount(-price);
            _eventBus.Publish(new ApplyEffectEvent(team, definitionId));
            return true;
        }

        private bool CanPurchaseCharacter(int price) =>
            _gameplayMoney.Amount >= price &&
            _charactersStorage.GetCharactersByTeam(Team.Player).Count() <
            _initialGameplayConfig.MaxPlayerCharactersCount;

        private bool CanPurchaseEffect(int price, Team team) =>
            _gameplayMoney.Amount >= price &&
            CountActiveEffectPurchases(team) < _initialGameplayConfig.MaxEffectsCount;
        
        private int CountActiveEffectPurchases(Team team)
        {
            var instanceIds = new HashSet<int>();
            
            foreach (var character in _charactersStorage.GetCharactersByTeam(team))
            {
                foreach (var effect in character.Effects)
                    instanceIds.Add(effect.InstanceId);
            }
            
            return instanceIds.Count;
        }
    }
}