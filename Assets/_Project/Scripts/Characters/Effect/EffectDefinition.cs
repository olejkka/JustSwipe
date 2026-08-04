using System;
using UnityEngine;

namespace _Project.Scripts.Characters.Effects
{
    [Serializable]
    public class EffectDefinition
    {
        [Header("Identity")]
        [SerializeField] private string _definitionId;
        [SerializeField] private EffectPolarity _polarity;
        [SerializeField] private EffectType _type;

        [Header("Gameplay")]
        [SerializeField] private int _parameter;
        [SerializeField] private int _turns;
        
        [Header("Presentation")]
        [SerializeField] private Sprite _icon;
        
        [Header("Economy")]
        [SerializeField] private int _price;
        
        public string DefinitionId => _definitionId;
        public EffectPolarity Polarity => _polarity;
        public EffectType Type => _type;
        public int Parameter => _parameter;
        public int Turns => _turns;
        public Sprite Icon => _icon;
        public int Price => _price;
    }
}