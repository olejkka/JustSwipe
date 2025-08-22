using _Project.Scripts.Infrastructure.FSM;
using _Project.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.LifetimeScopes
{
    public class GameplayUILifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PauseButtonPresenter>();

            builder.RegisterComponentInHierarchy<PauseButtonView>();
        }
    }
}