using _Project.Scripts.UI.CharacterCase;
using _Project.Scripts.UI.CheatsPanel;
using _Project.Scripts.UI.CheatsPanel.AddMoneyButton;
using _Project.Scripts.UI.GameplayStatistic;
using _Project.Scripts.UI.MoneyUI;
using _Project.Scripts.UI.SettingsButton;
using _Project.Scripts.UI.SettingsPopup;
using _Project.Scripts.UI.Shop;
using _Project.Scripts.UI.Shop.ShopRerollButton;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayUILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //Views
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<GameplaySettingsPopupView>();
            builder.RegisterComponentInHierarchy<CharacterCasesContainerView>();
            builder.RegisterComponentInHierarchy<ShopCaseView>();
            builder.RegisterComponentInHierarchy<ShopRerollButtonView>();
            builder.RegisterComponentInHierarchy<MoneyView>();
            builder.RegisterComponentInHierarchy<GameplayStatisticView>();
            
            //Presenters
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<GameplaySettingsPopupPresenter>();
            builder.RegisterEntryPoint<ShopCasePresenter>();
            builder.RegisterEntryPoint<ShopRerollButtonPresenter>();
            builder.RegisterEntryPoint<MoneyPresenter>();
            builder.RegisterEntryPoint<GameplayStatisticPresenter>();
            builder.RegisterEntryPoint<CharacterCasesContainerPresenter>();
            
            //Cheats
            builder.RegisterComponentInHierarchy<CheatsPanelView>();
            builder.RegisterEntryPoint<CheatsPanelPresenter>();
            builder.RegisterComponentInHierarchy<AddMoneyButtonView>();
            builder.RegisterEntryPoint<AddMoneyButtonPresenter>();
        }
    }
}