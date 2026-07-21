using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.UI.ApplyEffectButton;
using _Project.Scripts.UI.CharacterCase;
using _Project.Scripts.UI.CharacterPurchaseCase;
using _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton;
using _Project.Scripts.UI.GameplayStatistic;
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
            
            //Views
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<GameplaySettingsPopupView>();
            builder.RegisterComponentInHierarchy<CharacterCasesContainerView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseView>();
            builder.RegisterComponentInHierarchy<CharacterPurchaseCaseRerollButtonView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<GameplayStatisticView>();
            builder.RegisterComponentInHierarchy<ApplyEffectButtonView>();
            
            //Presenters
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<GameplaySettingsPopupPresenter>();
            builder.RegisterEntryPoint<CharacterPurchaseCasePresenter>();
            builder.RegisterEntryPoint<CharacterPurchaseCaseRerollButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
            builder.RegisterEntryPoint<GameplayStatisticPresenter>();
            builder.RegisterEntryPoint<ApplyEffectButtonPresenter>();
        }
    }
}