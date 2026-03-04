using _Project.Scripts.UI.PlayButton;
using _Project.Scripts.UI.SettingsButton;
using _Project.Scripts.UI.SettingsPopup;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class MenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //Views
            builder.RegisterComponentInHierarchy<PlayButtonView>();
            builder.RegisterComponentInHierarchy<SettingsButtonView>();
            builder.RegisterComponentInHierarchy<SettingsPopupView>();
            
            //Presenters
            builder.RegisterEntryPoint<PlayButtonPresenter>();
            builder.RegisterEntryPoint<SettingsButtonPresenter>();
            builder.RegisterEntryPoint<SettingsPopupPresenter>();
        }
    }
}