using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.ScriptableObjects;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCaseUI
{
    public class CharacterCasesManager : IStartable, IDisposable
    {
        private readonly CharacterCaseUIView[] _caseViews;
        private readonly CharacterCaseUIPresenter[] _casePresenters;
        private readonly CharactersStorage _charactersStorage;
        private readonly CharactersConfig _charactersConfig;

        private bool _initialized;

        public CharacterCasesManager(
            CharacterCaseUIView[] caseViews,
            CharactersStorage charactersStorage,
            CharactersConfig charactersConfig)
        {
            _caseViews = caseViews;
            _charactersStorage = charactersStorage;
            _charactersConfig = charactersConfig;

            _casePresenters = new CharacterCaseUIPresenter[_caseViews.Length];
        }

        public void Start()
        {
            EnsureInitialized();
        }

        public void Dispose()
        {
            for (int i = 0; i < _casePresenters.Length; i++)
            {
                _casePresenters[i]?.Dispose();
                _casePresenters[i] = null;
            }
        }

        public void OnCharacterCreated(Character character)
        {
            EnsureInitialized();

            if (character.Team != Team.Player) return;

            for (int i = 0; i < _casePresenters.Length; i++)
            {
                if (!_casePresenters[i].IsAssigned())
                {
                    _casePresenters[i].AssignCharacter(character);
                    return;
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            for (int i = 0; i < _caseViews.Length; i++)
            {
                _casePresenters[i] = new CharacterCaseUIPresenter(_caseViews[i], _charactersStorage, _charactersConfig);
                _casePresenters[i].Start();
            }

            _initialized = true;
        }
    }
}