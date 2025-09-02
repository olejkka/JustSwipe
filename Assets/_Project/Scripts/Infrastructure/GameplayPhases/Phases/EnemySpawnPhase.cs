using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Creators;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public class EnemySpawnPhase : Phase, IOrderedPhase
    {
        private const float SpawnChance = 0.25f;

        private readonly CharacterCreator _creator;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharacterConfig[] _botConfigs;

        public int Order => 1;

        
        public EnemySpawnPhase(
            CharacterCreator creator,
            CharactersStorage charactersStorage,
            CharacterConfig[] characterConfigs
        )
        {
            _creator = creator;
            _charactersStorage = charactersStorage;
            _botConfigs = characterConfigs.Where(c => c.Team == Team.Bot).ToArray();
        }

        public override void Enter()
        {
            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any() || Random.value <= SpawnChance)
                _creator.Create(_botConfigs[Random.Range(0, _botConfigs.Length)]);

            Exit();
        }
    }
}