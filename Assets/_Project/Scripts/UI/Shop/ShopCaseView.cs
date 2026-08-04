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
                    _effectCaseView.SetActive(false);

                    _characterCaseView.SetIcon(offer.Icon);
                    _characterCaseView.SetHealth(offer.Health);
                    _characterCaseView.SetDamage(offer.Damage);
                    _characterCaseView.SetBonusHealth(0);
                    _characterCaseView.SetBonusDamage(0);
                    break;

                case ShopOfferType.Effect:
                    _characterCaseView.SetActive(false);
                    _effectCaseView.SetActive(true);

                    _effectCaseView.SetIcon(offer.Icon);
                    _effectCaseView.SetBackgroundColor(offer.BackgroundColor);
                    
                    _effectCaseView.SetTurnsLeft(offer.Turns);
                    _effectCaseView.SetHealthBuffIcons(offer.EffectType == EffectType.HealthIncrease ? offer.EffectParameter : 0);
                    _effectCaseView.SetDamageBuffIcons(offer.EffectType == EffectType.DamageIncrease ? offer.EffectParameter : 0);
                    break;
            }
        }
    }
}