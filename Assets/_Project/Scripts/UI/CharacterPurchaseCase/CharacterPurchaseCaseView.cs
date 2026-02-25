using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseCaseView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private Button _purchaseButton;

        public event Action OnPurchaseClicked;

        private void OnEnable()
        {
            _purchaseButton.onClick.AddListener(HandlePurchaseClick);
        }

        private void OnDisable()
        {
            _purchaseButton.onClick.RemoveListener(HandlePurchaseClick);
        }

        public void SetData(Sprite icon, int price, int health, int damage)
        {
            _iconImage.sprite = icon;
                
            _priceText.text = $"${price}";
                
            _healthText.text = $"{health}";
            
            _damageText.text = $"{damage}";
        }

        private void HandlePurchaseClick() => OnPurchaseClicked?.Invoke();
    }
}