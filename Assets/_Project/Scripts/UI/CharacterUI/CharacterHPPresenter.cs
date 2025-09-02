using System;
using _Project.Scripts.Characters;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterUI
{
    public class CharacterHPPresenter : IDisposable
    {
        private readonly CharacterHPView _view;
        private readonly Character _character;
        private readonly Transform _characterTransform;
        private readonly Camera _camera;

        private int _maxHealth;
        private int _lastHealth;
        
        public CharacterHPView View => _view;
        

        public CharacterHPPresenter(Character character, CharacterHPView view)
        {
            _character = character;
            _view = view;
        }
        
        public void Initialize()
        {
            _character.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(Character character, int newHealth)
        {
            
        }

        public void Dispose()
        {
            _character.OnHealthChanged -= OnHealthChanged;
        }
    }
}