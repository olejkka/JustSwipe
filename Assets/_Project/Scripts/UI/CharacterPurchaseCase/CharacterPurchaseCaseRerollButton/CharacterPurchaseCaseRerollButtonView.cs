using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton
{
    public class CharacterPurchaseCaseRerollButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _priceText;
        
        public event Action OnRerollClicked;

        
        private void OnEnable()
        {
            _button.onClick.AddListener(HandleRerollClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleRerollClick);
        }
        
        public void SetData(int price)
        {
            _priceText.text = $"${price}";
        }
            
        private void HandleRerollClick() => OnRerollClicked?.Invoke();
    }
}