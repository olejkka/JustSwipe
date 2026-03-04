using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using VContainer.Unity;

namespace _Project.Scripts.Characters
{
    public class KillRewardHandler : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly GameplayMoney _gameplayMoney;
        private readonly CharactersConfig _charactersConfig;

        public KillRewardHandler(EventBus eventBus, GameplayMoney gameplayMoney, CharactersConfig charactersConfig)
        {
            _eventBus = eventBus;
            _gameplayMoney = gameplayMoney;
            _charactersConfig = charactersConfig;
        }

        public void Start() =>
            _eventBus.Subscribe<CharacterDiedEvent>(OnCharacterDied);

        public void Dispose() =>
            _eventBus.Unsubscribe<CharacterDiedEvent>(OnCharacterDied);

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Killer == null)
                return;
            
            if (e.Killer.Team != Team.Player) 
                return;
            
            if (e.Character.Team == Team.Player) 
                return;

            var entry = _charactersConfig.GetEntry(e.Character.CharacterType);
            _gameplayMoney.ChangeAmount(entry.Reward);
        }
    }
}