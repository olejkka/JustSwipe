using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Economy;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Characters
{
    public class CharactersDeathHandler : IDisposable
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly BotDeathRewardService _rewardService;
        private readonly List<Character> _registeredCharacters = new();
        

        public CharactersDeathHandler(
            CharactersStorage charactersStorage, 
            CharactersViewsStorage charactersViewsStorage,
            BotDeathRewardService rewardService
            )
        {
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
            _rewardService = rewardService;
        }

        public void Register(Character character)
        {
            if (_registeredCharacters.Contains(character))
                return;

            _registeredCharacters.Add(character);
            character.OnHealthChanged += OnHealthChanged;
        }

        public void Unregister(Character character)
        {
            if (_registeredCharacters.Remove(character))
                character.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(Character character, int newHealth)
        {
            if (newHealth > 0)
                return;

            HandleDeath(character);
        }

        private void HandleDeath(Character character)
        {
            Unregister(character);
            _charactersStorage.Remove(character);

            if (_charactersViewsStorage.TryGet(character, out var view) && view != null)
            {
                _charactersViewsStorage.Unregister(character);
                UnityEngine.Object.Destroy(view.gameObject);
            }
            
            if (character.Team == Team.Bot)
            {
                _rewardService.ProcessBotDeath(character.Id);
            }
        }

        public void Dispose()
        {
            foreach (var character in _registeredCharacters)
                character.OnHealthChanged -= OnHealthChanged;
            
            _registeredCharacters.Clear();
        }
    }
}