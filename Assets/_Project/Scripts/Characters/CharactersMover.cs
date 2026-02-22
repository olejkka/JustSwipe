using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.Events;
using _Project.Scripts.Tiles;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMover
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly EventBus _eventBus;
        private readonly Dictionary<Vector2Int, Character> _claimedPositions = new();

        
        public CharactersMover(
            CharactersStorage charactersStorage, 
            TilesPositionsStorage tilesPositionsStorage,
            EventBus eventBus)
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
            _eventBus = eventBus;
        }

        public void Move(Vector2Int vector, Team team)
        {
            _claimedPositions.Clear();

            var characters = _charactersStorage.GetAllCharacters().ToArray();
            
            for (int i = 0; i < characters.Length; i++)
                _claimedPositions[characters[i].Position] = characters[i];

            var attackers = _charactersStorage.GetCharactersByTeam(team).ToArray();
            
            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];
                var target = attacker.Position + vector;

                if (_claimedPositions.TryGetValue(target, out var defender))
                {
                    if (defender.Team != attacker.Team)
                    {
                        defender.TakeDamage(attacker.Damage, attacker);
                    }
                    
                    continue;
                }

                attacker.Move(vector);
            }

            KillCharactersOutOfBounds();

            if (team == Team.Player)
                _eventBus.Publish(new PlayerMoveCompletedEvent());
            else
                _eventBus.Publish(new BotMoveCompletedEvent());

            _eventBus.Publish(new CharactersMovedEvent());
        }
        
        private void KillCharactersOutOfBounds()
        {
            var allCharacters = _charactersStorage.GetAllCharacters().ToArray();
            
            for (int i = 0; i < allCharacters.Length; i++)
            {
                var character = allCharacters[i];
                
                if (!_tilesPositionsStorage.Contains(character.Position)) 
                    character.TakeDamage(character.Health);
            }
        }
    }
}