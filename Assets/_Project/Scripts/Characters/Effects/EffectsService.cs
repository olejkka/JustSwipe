using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Effects.EffectProcessors;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs___Enums;
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
        private readonly Dictionary<EffectType, IEffectProcessor> _processors;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        public EffectsService(
            EventBus eventBus, 
            CharactersStorage charactersStorage,
            IEnumerable<IEffectProcessor> processors
            )
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _processors = new Dictionary<EffectType, IEffectProcessor>();
            
            foreach (var processor in processors)
                _processors.Add(processor.Type, processor);
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
            {
                character.AddEffect(effect);
                
                if (_processors.TryGetValue(effect.Type, out var processor))
                    processor.Process(character, effect);
            }
        }

        private void OnTurnEnded(TurnEndedEvent e)
        {
            foreach (var character in _charactersStorage.GetCharactersByTeam(e.Team))
                character.TickEffects();
        }
    }
}