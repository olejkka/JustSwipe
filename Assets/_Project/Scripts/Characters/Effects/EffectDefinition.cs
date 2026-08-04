using System;
using UnityEngine;

namespace _Project.Scripts.Characters.Effects
{
    [Serializable]
    public class EffectDefinition
    {
        [Header("Identity")]
        [SerializeField] private string _definitionId;

        [Header("Gameplay")]
        [SerializeField] private EffectPolarity _polarity;
        [SerializeField] private EffectType _type;
        [SerializeField] private int _parameter;
        [SerializeField] private bool _isInstant;
        [SerializeField] private int _turns;

        [Header("Presentation")]
        [SerializeField] private Sprite _icon;
        
        [Header("Economy")]
        [SerializeField] private int _price;
        
        public string DefinitionId => _definitionId;
        public EffectPolarity Polarity => _polarity;
        public EffectType Type => _type;
        public int Parameter => _parameter;
        public bool IsInstant => _isInstant;
        public int Turns => _isInstant ? 0 : _turns;
        public Sprite Icon => _icon;
        public int Price => _price;
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_isInstant)
                _turns = 0;
            else if (_turns < 1)
                _turns = 1;
        }
#endif
    }
}