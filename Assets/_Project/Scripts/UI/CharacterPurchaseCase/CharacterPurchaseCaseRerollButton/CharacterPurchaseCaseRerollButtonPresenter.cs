using System;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton
{
    public class CharacterPurchaseCaseRerollButtonPresenter : IStartable, IDisposable
    {
        private readonly CharacterPurchaseCaseRerollButtonView _view;
        private readonly EventBus _eventBus;
        private readonly RerollPurchaseService _rerollPurchaseService;
        private readonly GameplayEconomyConfig _gameplayEconomyConfig;
        

        public CharacterPurchaseCaseRerollButtonPresenter(
            CharacterPurchaseCaseRerollButtonView view, 
            EventBus eventBus,
            RerollPurchaseService rerollPurchaseService,
            GameplayEconomyConfig gameplayEconomyConfig
            )
        {
            _view = view;
            _eventBus = eventBus;
            _rerollPurchaseService = rerollPurchaseService;
            _gameplayEconomyConfig = gameplayEconomyConfig;
        }
        
        public void Start()
        {
            _view.OnRerollClicked += OnRerollClicked;

            _view.SetData(_gameplayEconomyConfig.RerollPrice);
        }

        public void Dispose()
        {
            _view.OnRerollClicked -= OnRerollClicked;
        }

        private void OnRerollClicked()
        {
            if (!_rerollPurchaseService.TryPurchase(_gameplayEconomyConfig.RerollPrice))
            {
                _view.RotateShakeImage();
                return;
            }
                

            _view.RotateImage();
            _eventBus.Publish(new CharacterPurchaseCaseRerollEvent());
        }
    }
}