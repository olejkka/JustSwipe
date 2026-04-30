using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Scripts.UI.CharacterPurchaseCase.CharacterPurchaseCaseRerollButton
{
    public class CharacterPurchaseCaseRerollButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private IconRotateBehaviour _iconRotateBehaviour;
        
        
        public void Initialize(Lifetime lifetime, Action rerollClicked)
        {
            lifetime.BracketButton(_button, () => rerollClicked?.Invoke());
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
    }
}