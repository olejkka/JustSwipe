using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton
{
    public class CharacterPurchaseCaseRerollButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private IconRotateBehaviour _iconRotateBehaviour;
        
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
            _priceText.text = $"{price}";
        }

        public void RotateImage()
        {
            _iconRotateBehaviour.Play();
        }

        public void RotateShakeImage()
        {
            _iconRotateBehaviour.PlayShakeRotation();
        }
            
        private void HandleRerollClick() => OnRerollClicked?.Invoke();
    }
}