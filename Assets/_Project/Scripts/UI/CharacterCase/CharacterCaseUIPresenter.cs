using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIPresenter : IStartable, IDisposable
    {
        private readonly CharacterCaseUIView _view;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersConfig _charactersConfig;
        private Character _assignedCharacter;
        
        public CharacterCaseUIPresenter(
            CharacterCaseUIView view,
            CharactersStorage charactersStorage,
            CharactersConfig charactersConfig)
        {
            _view = view;
            _charactersStorage = charactersStorage;
            _charactersConfig = charactersConfig;
        }
        
        public void Start()
        {
            _view.SetActive(false);
        }
        
        public void Dispose()
        {
            if (_assignedCharacter != null)
            {
                _assignedCharacter.OnHealthChanged -= OnHealthChanged;
                _assignedCharacter = null;
            }
        }
        
        public void AssignCharacter(Character character)
        {
            if (character.Team != Team.Player)
            {
                Debug.LogWarning($"Trying to assign non-player character to case: {character.Team}");
                return;
            }
            
            if (_assignedCharacter != null)
            {
                _assignedCharacter.OnHealthChanged -= OnHealthChanged;
            }
            
            _assignedCharacter = character;
            
            _assignedCharacter.OnHealthChanged += OnHealthChanged;
            
            var entry = _charactersConfig.GetEntry(character.CharacterType);
            if (entry != null)
            {
                _view.SetIcon(entry.Sprite);
            }
            
            UpdateStats();
            
            _view.SetActive(true);
        }
        
        public void UnassignCharacter()
        {
            if (_assignedCharacter != null)
            {
                _assignedCharacter.OnHealthChanged -= OnHealthChanged;
                _assignedCharacter = null;
            }
            
            _view.SetActive(false);
        }
        
        private void OnHealthChanged(int newHealth)
        {
            UpdateStats();
        }
        
        private void UpdateStats()
        {
            if (_assignedCharacter == null) return;
            
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