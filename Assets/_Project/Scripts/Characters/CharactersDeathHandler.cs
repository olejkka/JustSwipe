using System;
using System.Collections.Generic;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Economy;

namespace _Project.Scripts.Characters
{
    public class CharactersDeathHandler : IDisposable
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        private readonly BotDeathRewardService _rewardService;
        private readonly CharactersMover _charactersMover;
        
        private readonly List<Character> _registeredCharacters = new();
        

        public CharactersDeathHandler(
            CharactersStorage charactersStorage, 
            CharactersViewsStorage charactersViewsStorage,
            BotDeathRewardService rewardService,
            CharactersMover charactersMover
            )
        {
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
            _rewardService = rewardService;
            _charactersMover = charactersMover;
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
            
            _charactersViewsStorage.TryGet(character, out var view);
            _charactersViewsStorage.Unregister(character);
            UnityEngine.Object.Destroy(view.gameObject);
            
            if (character.Team == Team.Bot)
            {
                var lastAttacker = _charactersMover.GetLastAttacker(character);
                
                if (lastAttacker != null && lastAttacker.Team == Team.Player)
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