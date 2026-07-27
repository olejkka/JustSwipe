using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Effects.EffectProcessors;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators.Generators;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Characters.Effects
{
    public class EffectsService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharacterEffectsConfig _characterEffectsConfig;
        private readonly InstanceIdGenerator _instanceIdGenerator;
        private readonly Dictionary<EffectType, IEffectProcessor> _processors;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public EffectsService(
            EventBus eventBus, 
            CharactersStorage charactersStorage,
            CharacterEffectsConfig characterEffectsConfig,
            InstanceIdGenerator instanceIdGenerator,
            IEnumerable<IEffectProcessor> processors)
        {
            _eventBus = eventBus;
            _charactersStorage = charactersStorage;
            _characterEffectsConfig = characterEffectsConfig;
            _instanceIdGenerator = instanceIdGenerator;
            _processors = new Dictionary<EffectType, IEffectProcessor>();
            
            foreach (var processor in processors)
                _processors.Add(processor.Type, processor);
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<ApplyEffectEvent>(_lifetimeDefinition.Lifetime, OnApplyEffect);
            _eventBus.SubscribeWithLifetime<TurnEndedEvent>(_lifetimeDefinition.Lifetime, OnTurnEnded);
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        private void OnApplyEffect(ApplyEffectEvent e)
        {
            var definition = _characterEffectsConfig.GetEntryByDefinitionId(e.DefinitionId);

            if (definition == null)
            {
                Debug.LogError($"Effect definition not found: {e.DefinitionId}");
                return;
            }
            
            var instanceId = _instanceIdGenerator.Next();
            
            foreach (var character in _charactersStorage.GetCharactersByTeam(e.Team))
            {
                var effect = new CharacterEffect(
                    definition.Type,
                    definition.Parameter,
                    definition.Turns,
                    definition.DefinitionId,
                    instanceId);
                
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