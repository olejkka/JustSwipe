using System;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.Characters.Effects
{
    public class EffectsService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        public EffectsService(EventBus eventBus, CharactersStorage charactersStorage)
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<ApplyEffectEvent>(_lifetimeDefinition.Lifetime, OnApplyEffect);
            _eventBus.SubscribeWithLifetime<TurnEndedEvent>(_lifetimeDefinition.Lifetime, OnTurnEnded);
        }

        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnApplyEffect(ApplyEffectEvent e)
        {
            var effect = new CharacterEffect(e.EffectType, e.Parameter, e.Turns);

            foreach (var character in _charactersStorage.GetCharactersByTeam(e.Team))
                character.AddEffect(effect);
        }

        private void OnTurnEnded(TurnEndedEvent e)
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(e.Team))
                character.TickEffects();
        }
    }
}