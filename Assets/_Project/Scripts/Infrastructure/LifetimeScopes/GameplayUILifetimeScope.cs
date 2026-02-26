using _Project.Scripts.UI.CharacterCase;
using _Project.Scripts.UI.CharacterPurchaseCase;
using _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton;
using _Project.Scripts.UI.MoneyUI;
using _Project.Scripts.UI.SettingsButton;
using _Project.Scripts.UI.SettingsPopup;
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
            builder.Register<RerollPurchaseService>(Lifetime.Singleton);
            
            //Views
            builder.RegisterComponentInHierarchy<CharacterCasesContainerView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseRerollButtonView>();
            
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<SettingsPopupView>();
            
            //Presenters
            builder.RegisterEntryPoint<CharacterPurchaseCasePresenter>();
            builder.RegisterEntryPoint<CharacterPurchaseCaseRerollButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
            
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<SettingsPopupPresenter>();
        }
    }
}