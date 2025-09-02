using _Project.Scripts.UI.MoneyUI;
using _Project.Scripts.UI.SettingsButton;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayUILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterPresenters(builder);
            RegisterViews(builder);
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<PlayerMoneyPresenter>();
        }
        
        private void RegisterViews(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<PlayerMoneyView>();
        }
    }
}