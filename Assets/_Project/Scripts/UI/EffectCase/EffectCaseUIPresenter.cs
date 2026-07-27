using System;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs___Enums;
using _Project.Scripts.Configs;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.EffectCase
{
    public class EffectCaseUIPresenter : IStartable, IDisposable
    {
        private readonly LifetimeDefinition _lifetimeDefinition;
        private readonly EffectCaseUIView _view;
        private readonly CharacterEffectsConfig _characterEffectsConfig;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;

        private LifetimeDefinition _assignmentLifetimeDefinition;
        private CharacterEffect _assignedEffect;
        private bool _hasAssignedEffect;

        
        public EffectCaseUIPresenter(
            Lifetime parentLifetime,
            EffectCaseUIView view,
            CharacterEffectsConfig characterEffectsConfig,
            CharactersStorage charactersStorage,
            CharactersViewsStorage charactersViewsStorage)
        {
            _lifetimeDefinition = parentLifetime.CreateNested();
            _view = view;
            _characterEffectsConfig = characterEffectsConfig;
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
        }

        public void Start()
        {
            _view.SetActive(false);
        }

        public void Dispose()
        {
            _assignmentLifetimeDefinition?.Terminate();
            _assignmentLifetimeDefinition = null;
            _lifetimeDefinition.Terminate();
            _hasAssignedEffect = false;
        }

        public void AssignEffect(CharacterEffect effect)
        {
            UnassignEffect();

            _assignedEffect = effect;
            _hasAssignedEffect = true;

            _assignmentLifetimeDefinition = _lifetimeDefinition.Lifetime.CreateNested();
            _view.BindClick(_assignmentLifetimeDefinition.Lifetime, OnCaseClicked);

            UpdateView();
            _view.SetActive(true);
        }

        public void UnassignEffect()
        {
            _assignmentLifetimeDefinition?.Terminate();
            _assignmentLifetimeDefinition = null;
            _hasAssignedEffect = false;
            _view.SetActive(false);
        }

        public bool IsAssigned() => _hasAssignedEffect;

        public bool IsAssignedTo(string definitionId) =>
            _hasAssignedEffect && _assignedEffect.DefinitionId == definitionId;

        public void Refresh(CharacterEffect effect)
        {
            if (!_hasAssignedEffect)
                return;

            _assignedEffect = effect;
            UpdateView();
        }

        private void UpdateView()
        {
            var entry = _characterEffectsConfig.GetEntryByDefinitionId(_assignedEffect.DefinitionId);
            _view.SetIcon(entry != null ? entry.Icon : null);

            if (entry == null)
                Debug.LogError($"No effect definition: {_assignedEffect.DefinitionId}");

            _view.SetTurnsLeft(_assignedEffect.RemainingTurns);

            _view.SetHealthBuffIcons(
                _assignedEffect.Type == EffectType.HealthIncrease ? _assignedEffect.Parameter : 0);
            _view.SetDamageBuffIcons(
                _assignedEffect.Type == EffectType.DamageIncrease ? _assignedEffect.Parameter : 0);
        }

        private void OnCaseClicked()
        {
            if (!_hasAssignedEffect)
                return;

            var instanceId = _assignedEffect.InstanceId;

            foreach (var character in _charactersStorage.GetCharactersByTeam(Team.Player))
            {
                if (!character.Effects.Any(e => e.InstanceId == instanceId))
                    continue;

                if (_charactersViewsStorage.TryGet(character, out var characterView) && characterView != null)
                    characterView.PlaySelected();
            }
        }
    }
}