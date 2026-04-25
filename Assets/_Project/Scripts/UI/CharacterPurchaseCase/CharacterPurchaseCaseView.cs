using System;
using _Project.Scripts.UI.CharacterCase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterPurchaseCase
{
    public class CharacterPurchaseCaseView : MonoBehaviour
    {
        [SerializeField] private CharacterCaseUIView _characterCaseView;
        [SerializeField] private TMP_Text _priceText;
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
            _characterCaseView.SetIcon(icon);
            _characterCaseView.SetHealth(health);
            _characterCaseView.SetDamage(damage);

            _priceText.text = $"{price}";
        }

        private void HandlePurchaseClick() => OnPurchaseClicked?.Invoke();
    }
}