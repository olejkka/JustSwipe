using System;
using _Project.Scripts.Characters.Effects;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.EffectCase
{
    public class EffectCaseUIView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Header("Icon")]
        [SerializeField] private Image _effectIcon;

        [Header("Background")]
        [SerializeField] private Image _background;

        [Header("Timer")]
        [SerializeField] private GameObject _turnsLeftContainer;
        [SerializeField] private TextMeshProUGUI _turnsLeftText;

        [Header("Effect Type Roots")]
        [SerializeField] private GameObject _healthIncrease;
        [SerializeField] private GameObject _damageIncrease;
        [SerializeField] private GameObject _damageDecrease;
        [SerializeField] private GameObject _instantHeal;
        [SerializeField] private GameObject _instantDamage;

        [Header("Effect Type Values")]
        [SerializeField] private TextMeshProUGUI _healthIncreaseText;
        [SerializeField] private TextMeshProUGUI _damageIncreaseText;
        [SerializeField] private TextMeshProUGUI _damageDecreaseText;
        [SerializeField] private TextMeshProUGUI _instantHealText;
        [SerializeField] private TextMeshProUGUI _instantDamageText;

        
        public void BindClick(Lifetime lifetime, Action onClick)
        {
            lifetime.BracketButton(_button, () => onClick?.Invoke());
        }

        public void SetIcon(Sprite sprite)
        {
            _effectIcon.sprite = sprite;
        }

        public void SetBackgroundColor(Color color)
        {
            _background.color = color;
        }

        public void SetEffectData(EffectType type, int parameter, int remainingTurns)
        {
            var parameterText = parameter.ToString();

            _healthIncrease.SetActive(type == EffectType.HealthIncrease);
            _damageIncrease.SetActive(type == EffectType.DamageIncrease);
            _damageDecrease.SetActive(type == EffectType.DamageDecrease);
            _instantHeal.SetActive(type == EffectType.Heal);
            _instantDamage.SetActive(type == EffectType.DealDamage);

            switch (type)
            {
                case EffectType.HealthIncrease:
                    _healthIncreaseText.text = parameterText;
                    break;
                case EffectType.DamageIncrease:
                    _damageIncreaseText.text = parameterText;
                    break;
                case EffectType.DamageDecrease:
                    _damageDecreaseText.text = parameterText;
                    break;
                case EffectType.Heal:
                    _instantHealText.text = parameterText;
                    break;
                case EffectType.DealDamage:
                    _instantDamageText.text = parameterText;
                    break;
            }

            var hasTurns = remainingTurns > 0;
            _turnsLeftContainer.SetActive(hasTurns);
            
            if (hasTurns)
                _turnsLeftText.text = remainingTurns.ToString();
        }
    }
}