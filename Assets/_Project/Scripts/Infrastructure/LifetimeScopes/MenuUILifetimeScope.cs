using _Project.Scripts.UI.PlayButton;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class MenuUILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //Views
            builder.RegisterComponentInHierarchy<PlayButtonView>();
            
            //Presenters
            builder.RegisterEntryPoint<PlayButtonPresenter>();
        }
    }
}