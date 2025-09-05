using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public class CharactersSpawnPhase : Phase
    {
        private const float SpawnChance = 0.25f;
		
        private readonly CharacterCreator _creator;
        private readonly CharactersStorage _charactersStorage;
        private readonly IReadOnlyList<CharacterConfig> _characterConfigs;
		

        public CharactersSpawnPhase(
            CharacterCreator creator,
            CharactersStorage charactersStorage,
            IReadOnlyList<CharacterConfig> characterConfigs
        )
        {
            _creator = creator;
            _charactersStorage = charactersStorage;
            _characterConfigs = characterConfigs;
        }
		
        public override void Enter()
        {
            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any() || Random.value < SpawnChance)
            {
                var botConfigs = _characterConfigs.Where(c => c.Team == Team.Bot).ToList();
                
                _creator.Create(botConfigs[Random.Range(0, botConfigs.Count)]);
            }
			
            Exit();
        }
    }
}