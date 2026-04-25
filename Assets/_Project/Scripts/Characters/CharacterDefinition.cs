using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using UnityEngine;
using UnityEngine.UI;

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
        public Sprite Icon => _icon;
        public CharacterAnimationData Animations => _animations;
        public int Price => _price;
        public int Reward => _reward;
    }
    
    [Serializable]
    public class CharacterAnimationData
    {
        public float FrameRate = 8f;
        public Sprite[] Idle;
        public Sprite[] Move;
        public Sprite[] DealDamage;
        public Sprite[] TakeDamage;
        public Sprite[] Death;
    }
}