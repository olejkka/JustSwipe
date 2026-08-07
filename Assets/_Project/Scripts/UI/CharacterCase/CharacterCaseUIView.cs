using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        [Header("Icons")]
        [SerializeField] private Image _characterIcon;
        [SerializeField] private Image _hpIcon;
        [SerializeField] private Image _damageIcon;
        
        [Header("Containers")]
        [SerializeField] private RectTransform _hpContainer;
        [SerializeField] private RectTransform _damageContainer;

        [Header("Colors")]
        [SerializeField] private Color _bonusStatColor;

        private readonly List<Image> _hpIconsPool = new();
        private readonly List<Image> _damageIconsPool = new();

        
        public void BindClick(Lifetime lifetime, Action onClick)
        {
            lifetime.BracketButton(_button, () => onClick?.Invoke());
        }
        
        public void SetIcon(Sprite sprite)
        {
            _characterIcon.sprite = sprite;
        }

        public void SetHealth(int health, int bonusHealth)
        {
            ShowIcons(
                _hpIconsPool,
                _hpContainer,
                _hpIcon,
                Mathf.Max(0, health),
                Mathf.Max(0, bonusHealth));
        }

        public void SetDamage(int damage, int bonusDamage)
        {
            ShowIcons(
                _damageIconsPool,
                _damageContainer,
                _damageIcon,
                Mathf.Max(0, damage),
                Mathf.Max(0, bonusDamage));
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void ShowIcons(
            List<Image> pool,
            RectTransform container,
            Image iconPrefab,
            int baseCount,
            int bonusCount)
        {
            var requiredCount = baseCount + bonusCount;
            var baseColor = iconPrefab.color;

            while (pool.Count < requiredCount)
            {
                var icon = Instantiate(iconPrefab, container);
                
                icon.gameObject.SetActive(false);
                pool.Add(icon);
            }

            for (var i = 0; i < requiredCount; i++)
            {
                var icon = pool[i];
                
                icon.color = i < baseCount ? baseColor : _bonusStatColor;
                icon.transform.SetSiblingIndex(i + 1);
                
                if (!icon.gameObject.activeSelf)
                    icon.gameObject.SetActive(true);
            }

            for (var i = requiredCount; i < pool.Count; i++)
            {
                var icon = pool[i];
                
                icon.transform.SetSiblingIndex(i + 1);
                
                if (icon.gameObject.activeSelf)
                    icon.gameObject.SetActive(false);
            }

            iconPrefab.gameObject.SetActive(false);
            iconPrefab.transform.SetSiblingIndex(0);
        }
    }
}