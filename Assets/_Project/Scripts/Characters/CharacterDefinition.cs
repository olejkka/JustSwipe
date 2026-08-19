using System;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    [Serializable]
    public class CharacterDefinition
    {
        [Header("Identity")]
        [SerializeField] private string _definitionId;
        [SerializeField] private CharacterType _type;
        
        [Header("Gameplay")]
        [SerializeField] private Team _team;
        [SerializeField] private CharacterBaseStats _baseStats;
        [SerializeField] private bool _isRanged;
        [SerializeField] private string _projectileDefinitionId;
        
        [Header("Presentation")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private CharacterAnimationData _animations;
        
        [Header("Economy")]
        [SerializeField] private int _price;
        [SerializeField] private int _reward;
        
        
        public string DefinitionId => _definitionId;
        public CharacterType CharacterType => _type;
        public Team Team => _team;
        public CharacterBaseStats BaseStats => _baseStats;
        public bool IsRanged => _isRanged;
        public Sprite Icon => _icon;
        public CharacterAnimationData Animations => _animations;
        public string ProjectileDefinitionId => _projectileDefinitionId;
        public int Price => _price;
        public int Reward => _reward;
    }
    
    [Serializable]
    public class CharacterAnimationData
    {
        public float FrameRate = 8f;
        public Sprite[] Idle;
        public Sprite[] Move;
        public Sprite[] MeleeAttack;
        public Sprite[] TakeDamage;
        public Sprite[] Death;
        public Sprite[] Selected;
        
        [Header("Selected Parameters")]
        public float SelectedJumpHeight = 0.3f;
        public float SelectedJumpDuration = 0.1f;
        public Ease SelectedJumpEaseUp = Ease.OutQuad;
        public Ease SelectedJumpEaseDown = Ease.InQuad;
    }
}