using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIView : MonoBehaviour
    {
        [SerializeField] private Image _characterIcon;
        [SerializeField] private RectTransform _hpContainer;
        [SerializeField] private RectTransform _damageContainer;
        [SerializeField] private Image _hpIconPrefab;
        [SerializeField] private Image _damageIconPrefab;

        private readonly List<Image> _hpIconsPool = new();
        private readonly List<Image> _damageIconsPool = new();

        
        public void SetIcon(Sprite sprite)
        {
            _characterIcon.sprite = sprite;
        }

        public void SetHealth(int health)
        {
            ShowIcons(_hpIconsPool, _hpContainer, _hpIconPrefab, Mathf.Max(0, health));
        }

        public void SetDamage(int damage)
        {
            ShowIcons(_damageIconsPool, _damageContainer, _damageIconPrefab, Mathf.Max(0, damage));
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private static void ShowIcons(List<Image> pool, RectTransform container, Image iconPrefab, int requiredCount)
        {
            while (pool.Count < requiredCount)
            {
                var icon = Instantiate(iconPrefab, container);
                icon.gameObject.SetActive(false);
                pool.Add(icon);
            }
            
            for (int i = 0; i < requiredCount; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                    pool[i].gameObject.SetActive(true);
            }
            
            for (int i = requiredCount; i < pool.Count; i++)
            {
                if (pool[i].gameObject.activeSelf)
                    pool[i].gameObject.SetActive(false);
            }
        }
    }
}