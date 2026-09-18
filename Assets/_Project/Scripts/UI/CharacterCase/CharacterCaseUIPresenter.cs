using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIPresenter : IStartable, IDisposable
    {
        private readonly LifetimeDefinition _lifetimeDefinition;
        private readonly CharacterCaseUIView _view;
        private readonly CharactersConfig _charactersConfig;
        private readonly CharacterCaseColorsConfig _colorsConfig;
        private readonly CharactersViewsStorage _charactersViewsStorage;
        
        private LifetimeDefinition _assignmentLifetimeDefinition;
        private Character _assignedCharacter;
        private CharacterView _characterView;
        private CharacterAnimationData _animations;
        private bool _unassignPending;
        
        
        public CharacterCaseUIPresenter(
            Lifetime parentLifetime,
            CharacterCaseUIView view,
            CharactersConfig charactersConfig,
            CharactersViewsStorage charactersViewsStorage,
            CharacterCaseColorsConfig colorsConfig)
        {
            _lifetimeDefinition = parentLifetime.CreateNested();
            _view = view;
            _charactersConfig = charactersConfig;
            _charactersViewsStorage = charactersViewsStorage;
            _colorsConfig = colorsConfig;
        }
        
        public void Start()
        {
            _view.SetActive(false);
        }
        
        public void Dispose()
        {
            _unassignPending = false;
            _characterView = null;
            _animations = null;
            
            _assignmentLifetimeDefinition?.Terminate();
            _assignmentLifetimeDefinition = null;
            
            _lifetimeDefinition.Terminate();
            _assignedCharacter = null;
        }
        
        public void AssignCharacter(Character character)
        {
            ExecuteUnassign();
            
            _assignedCharacter = character;
            var assigned = character;
            
            _assignmentLifetimeDefinition = _lifetimeDefinition.Lifetime.CreateNested();
            var lifetime = _assignmentLifetimeDefinition.Lifetime;
            
            _view.BindClick(lifetime, OnCaseClicked);
            
            lifetime.Bracket(
                () => assigned.OnStatsChanged += OnStatsChanged,
                () => assigned.OnStatsChanged -= OnStatsChanged);
            
            var entry = _charactersConfig.GetEntryByDefinitionId(character.DefinitionId);
            
            if (entry != null)
            {
                _animations = entry.Animations;
                _view.SetAnimations(_animations);
                _view.SetIcon(entry.Icon);
            }
            
            UpdateStats();
            _view.SetActive(true);
            _view.UpdateRotation(character.Team);
            _view.SetBackgroundColor(_colorsConfig.GetBackgroundColor(character.Team));
            
            BindCharacterView(lifetime, character);
        }
        
        public void UnassignCharacter()
        {
            if (_unassignPending)
                return;

            if (_assignedCharacter == null || !HasDeathFrames())
            {
                ExecuteUnassign();
                return;
            }

            _unassignPending = true;

            if (_view.CurrentAnimationType == CharacterAnimationType.Death)
                return;

            if (_characterView == null)
                _view.PlayDeath(OnCaseDeathFinished);
        }
        
        public bool IsAssigned()
        {
            return _assignedCharacter != null;
        }

        public bool IsAssignedTo(Character character)
        {
            return _assignedCharacter != null && ReferenceEquals(_assignedCharacter, character);
        }

        private void ExecuteUnassign()
        {
            _unassignPending = false;
            _characterView = null;
            _animations = null;
            _view.StopAnimation();

            _assignmentLifetimeDefinition?.Terminate();
            _assignmentLifetimeDefinition = null;
            
            _assignedCharacter = null;
            _view.SetActive(false);
        }

        private void BindCharacterView(Lifetime lifetime, Character character)
        {
            if (_charactersViewsStorage.TryGet(character, out var view) && view != null)
            {
                SubscribeToCharacterView(lifetime, view);
                return;
            }

            lifetime.BracketSubscription(
                () => _charactersViewsStorage.OnRegistered += OnViewRegistered,
                () => _charactersViewsStorage.OnRegistered -= OnViewRegistered);
        }

        private void OnViewRegistered(Character character, CharacterView view)
        {
            if (!IsAssignedTo(character) || view == null || _assignmentLifetimeDefinition == null)
                return;

            SubscribeToCharacterView(_assignmentLifetimeDefinition.Lifetime, view);
        }

        private void SubscribeToCharacterView(Lifetime lifetime, CharacterView view)
        {
            if (ReferenceEquals(_characterView, view))
                return;

            _characterView = view;

            lifetime.BracketSubscription(
                () => view.OnAnimationStarted += OnCharacterAnimationStarted,
                () =>
                {
                    if (view != null)
                        view.OnAnimationStarted -= OnCharacterAnimationStarted;
                });

            MirrorAnimation(view.CurrentAnimationType);
        }

        private void OnCharacterAnimationStarted(CharacterAnimationType animationType) => 
            MirrorAnimation(animationType);

        private void MirrorAnimation(CharacterAnimationType animationType)
        {
            if (_unassignPending && animationType != CharacterAnimationType.Death)
                return;

            switch (animationType)
            {
                case CharacterAnimationType.Idle:
                    _view.PlayIdle();
                    break;
                case CharacterAnimationType.TakingDamage:
                    _view.PlayTakingDamage();
                    break;
                case CharacterAnimationType.Death:
                    _view.PlayDeath(OnCaseDeathFinished);
                    break;
            }
        }

        private void OnCaseDeathFinished()
        {
            if (!_unassignPending)
                return;

            ExecuteUnassign();
        }
        
        private void OnStatsChanged()
        {
            UpdateStats();
        }
        
        private void OnCaseClicked()
        {
            if (_assignedCharacter == null || _unassignPending)
                return;

            if (_view.CurrentAnimationType == CharacterAnimationType.Death)
                return;

            if (_characterView == null)
                return;

            if (_characterView.CurrentAnimationType == CharacterAnimationType.Death)
                return;

            _characterView.PlaySelected();
        }
        
        private void UpdateStats()
        {
            if (_assignedCharacter == null) 
                return;
    
            _view.SetHealth(_assignedCharacter.Health, _assignedCharacter.BonusHealth);
            _view.SetDamage(_assignedCharacter.Damage, _assignedCharacter.BonusDamage);
        }

        private bool HasDeathFrames()
        {
            return _animations != null &&
                   _animations.Death != null &&
                   _animations.Death.Length > 0;
        }
    }
}
