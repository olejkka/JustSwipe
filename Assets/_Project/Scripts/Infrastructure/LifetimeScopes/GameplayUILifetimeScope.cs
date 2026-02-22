using _Project.Scripts.UI.CharacterCase;
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
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CharacterCasesContainerPresenter>();
            builder.Register<CharacterPurchaseService>(Lifetime.Singleton);
            
            //Views
            builder.RegisterComponentInHierarchy<CharacterCasesContainerView>();
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseView>();
            
            //Presenters
            builder.RegisterEntryPoint<CharacterPurchaseCasePresenter>();
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
        }
    }
}