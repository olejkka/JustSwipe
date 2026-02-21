using _Project.Scripts.UI.CharacterCaseUI;
using _Project.Scripts.UI.CharacterPurchaseCase;
using _Project.Scripts.UI.MoneyUI;
using _Project.Scripts.UI.SettingsButton;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayUILifetimeScope : LifetimeScope
    {
        [SerializeField] private CharacterCaseUIView[] _characterCaseViews;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_characterCaseViews);
            
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseView>();
            
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
            builder.RegisterEntryPoint<CharacterPurchaseCasePresenter>();
            
            builder.RegisterEntryPoint<CharacterCasesManager>();
        }
    }
}