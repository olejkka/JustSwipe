using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public class DefeatPhase : Phase, IOrderedPhase
    {
        private readonly CharactersStorage _charactersStorage;
        public int Order => 3;

        
        public DefeatPhase(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public override void Enter()
        {
            if (!_charactersStorage.GetCharactersByTeam(Team.Player).Any())
            {
                Time.timeScale = 0f;
                Debug.Log("Поражение");
                return;
            }

            Exit();
        }
    }
}