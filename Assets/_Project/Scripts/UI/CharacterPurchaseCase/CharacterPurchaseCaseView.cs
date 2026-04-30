using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.UI.CharacterCase;
using JetBrains.Lifetimes;
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

        
        public void Initialize(Lifetime lifetime, Action purchaseClicked)
        {
            lifetime.BracketButton(_purchaseButton, () => purchaseClicked?.Invoke());
        }

        public void SetData(Sprite icon, int price, int health, int damage)
        {
            _characterCaseView.SetIcon(icon);
            _characterCaseView.SetHealth(health);
            _characterCaseView.SetDamage(damage);

            _priceText.text = $"{price}";
        }
    }
}