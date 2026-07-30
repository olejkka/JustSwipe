using System;
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

        [Header("Icons")]
        [SerializeField] private Image _effectIcon;
        [SerializeField] private Image _hpEffectIcon;
        [SerializeField] private Image _damageEffectIcon;

        [Header("Background")]
        [SerializeField] private Image _background;
        
        [Header("Timer")]
        [SerializeField] private TextMeshProUGUI _turnsLeftText;

        [Header("Containers")]
        [SerializeField] private RectTransform _hpEffectIconsContainer;
        [SerializeField] private RectTransform _damageEffectIconsContainer;

        
        public void BindClick(Lifetime lifetime, Action onClick)
        {
            lifetime.BracketButton(_button, () => onClick?.Invoke());
        }

        public void SetIcon(Sprite sprite)
        {
            _effectIcon.sprite = sprite;
        }

        public void SetHealthBuffIcons(int health)
        {
            ShowIcons(_hpEffectIconsContainer, _hpEffectIcon, Mathf.Max(0, health));
        }

        public void SetDamageBuffIcons(int damage)
        {
            ShowIcons(_damageEffectIconsContainer, _damageEffectIcon, Mathf.Max(0, damage));
        }
        
        public void SetBackgroundColor(Color color)
        {
            _background.color = color;
        }

        public void SetTurnsLeft(int turnsLeft)
        {
            _turnsLeftText.text = turnsLeft.ToString();
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private static void ShowIcons(RectTransform container, Image iconPrefab, int requiredCount)
        {
            for (var i = container.childCount - 1; i >= 0; i--)
            {
                var child = container.GetChild(i).gameObject;
                
                if (child == iconPrefab.gameObject)
                    continue;
                
                Destroy(child);
            }
            for (var i = 0; i < requiredCount; i++)
            {
                var icon = Instantiate(iconPrefab, container);
                icon.gameObject.SetActive(true);
            }
            iconPrefab.gameObject.SetActive(false);
            container.gameObject.SetActive(requiredCount > 0);
        }
    }
}