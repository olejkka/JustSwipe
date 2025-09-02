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
        private readonly DeathRewardService _rewardService;
        private readonly CharactersCombatHandler _combatHandler;
        
        private readonly List<Character> _registeredCharacters = new();
        

        public CharactersDeathHandler(
            CharactersStorage charactersStorage, 
            CharactersViewsStorage charactersViewsStorage,
            DeathRewardService rewardService,
            CharactersCombatHandler combatHandler
            )
        {
            _charactersStorage = charactersStorage;
            _charactersViewsStorage = charactersViewsStorage;
            _rewardService = rewardService;
            _combatHandler = combatHandler;
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
            
            if (character.CharacterConfig.Team == Team.Bot)
            {
                var lastAttacker = _combatHandler.GetLastAttacker(character);
                
                if (lastAttacker != null && lastAttacker.CharacterConfig.Team == Team.Player)
                    _rewardService.GiveReward(character);
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