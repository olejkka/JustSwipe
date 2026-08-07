using System;
using _Project.Scripts.Characters.Effects;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.UI.CharacterCase;
using _Project.Scripts.UI.EffectCase;
using JetBrains.Lifetimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Shop
{
    public class ShopCaseView : MonoBehaviour
    {
        [SerializeField] private CharacterCaseUIView _characterCaseView;
        [SerializeField] private EffectCaseUIView _effectCaseView;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _purchaseButton;

        
        public void Initialize(Lifetime lifetime, Action purchaseClicked)
        {
            lifetime.BracketButton(_purchaseButton, () => purchaseClicked?.Invoke());
        }

        public void SetData(ShopOffer offer)
        {
            _priceText.text = $"{offer.Price}";

            switch (offer.Type)
            {
                case ShopOfferType.Character:
                    _characterCaseView.SetActive(true);
                    _effectCaseView.gameObject.SetActive(false);

                    _characterCaseView.SetIcon(offer.Icon);
                    _characterCaseView.SetHealth(offer.Health, 0);
                    _characterCaseView.SetDamage(offer.Damage, 0);
                    break;

                case ShopOfferType.Effect:
                    _characterCaseView.SetActive(false);
                    _effectCaseView.gameObject.SetActive(true);

                    _effectCaseView.SetIcon(offer.Icon);
                    _effectCaseView.SetBackgroundColor(offer.BackgroundColor);
                    
                    _effectCaseView.SetEffectData(offer.EffectType, offer.EffectParameter, offer.Turns);
                    break;
            }
        }
    }
}