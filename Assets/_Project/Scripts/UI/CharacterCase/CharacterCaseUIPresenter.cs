using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIPresenter : IStartable, IDisposable
    {
        private readonly LifetimeDefinition _lifetimeDefinition;
        private readonly CharacterCaseUIView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        
        private LifetimeDefinition _assignmentLifetimeDefinition;
        private Character _assignedCharacter;
        
        
        public CharacterCaseUIPresenter(
            Lifetime parentLifetime,
            CharacterCaseUIView view,
            CharactersConfig charactersConfig,
            CharactersViewsStorage charactersViewsStorage)
        {
            _lifetimeDefinition = parentLifetime.CreateNested();
            _view = view;
            _charactersConfig = charactersConfig;
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
            _assignedCharacter = null;
        }
        
        public void AssignCharacter(Character character)
        {
            if (character.Team != Team.Player)
            {
                Debug.LogWarning($"Trying to assign non-player character to case: {character.Team}");
                return;
            }
            
            UnassignCharacter();
            
            _assignedCharacter = character;
            _assignmentLifetimeDefinition = _lifetimeDefinition.Lifetime.CreateNested();
            _view.BindClick(_assignmentLifetimeDefinition.Lifetime, OnCaseClicked);
            
            _assignmentLifetimeDefinition.Lifetime.Bracket(
                () => _assignedCharacter.OnHealthChanged += OnHealthChanged,
                () => _assignedCharacter.OnHealthChanged -= OnHealthChanged);
            
            var entry = _charactersConfig.GetEntryByDefinitionId(character.DefinitionId);
            
            if (entry != null)
                _view.SetIcon(entry.Icon);
            
            UpdateStats();
            _view.SetActive(true);
        }
        
        public void UnassignCharacter()
        {
            _assignmentLifetimeDefinition?.Terminate();
            _assignmentLifetimeDefinition = null;
            
            _assignedCharacter = null;
            _view.SetActive(false);
        }
        
        private void OnHealthChanged(int newHealth)
        {
            UpdateStats();
        }
        
        private void OnCaseClicked()
        {
            if (_assignedCharacter == null)
                return;
            
            if (_charactersViewsStorage.TryGet(_assignedCharacter, out var view) && view != null)
                view.PlaySelected();
        }
        
        private void UpdateStats()
        {
            if (_assignedCharacter == null) 
                return;
            
            _view.SetHealth(_assignedCharacter.Health);
            _view.SetDamage(_assignedCharacter.Damage);
        }

        public bool IsAssigned()
        {
            return _assignedCharacter != null;
        }

        public bool IsAssignedTo(Character character)
        {
            return _assignedCharacter != null && ReferenceEquals(_assignedCharacter, character);
        }
    }
}