using System;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.ApplyEffectButton
{
    public class ApplyEffectButtonPresenter : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly ApplyEffectButtonView _view;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public ApplyEffectButtonPresenter(EventBus eventBus, ApplyEffectButtonView view)
        {
            _eventBus = eventBus;
            _view = view;
        }

        public void Start() =>
            _view.Initialize(_lifetimeDefinition.Lifetime, OnApplyEffectClicked);

        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnApplyEffectClicked()
        {
            _eventBus.Publish(new ApplyEffectEvent(
                Team.Player,
                EffectType.HealthIncrease,
                parameter: 1,
                turns: 2));
        }
    }
}