using System;
using _Project.Scripts.GameplayEconomy;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.UI.CharacterCase;
using _Project.Scripts.UI.Common;
using _Project.Scripts.UI.EffectCase;
using JetBrains.Lifetimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Shop
{
    public class ShopView : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private CharacterCaseUIView _characterCaseView;
        [SerializeField] private Button _characterPurchaseButton;
        [SerializeField] private Button _characterRerollButton;
        [SerializeField] private TMP_Text _characterPriceText;
        [SerializeField] private TMP_Text _characterRerollPriceText;
        [SerializeField] private IconRotateBehaviour _characterRerollIcon;

        [Header("Effect")]
        [SerializeField] private EffectCaseUIView _effectCaseView;
        [SerializeField] private Button _effectPurchaseButton;
        [SerializeField] private Button _effectRerollButton;
        [SerializeField] private TMP_Text _effectPriceText;
        [SerializeField] private TMP_Text _effectRerollPriceText;
        [SerializeField] private IconRotateBehaviour _effectRerollIcon;

        
        public void Initialize(
            Lifetime lifetime,
            Action characterPurchaseClicked,
            Action characterRerollClicked,
            Action effectPurchaseClicked,
            Action effectRerollClicked)
        {
            lifetime.BracketButton(_characterPurchaseButton, () => characterPurchaseClicked?.Invoke());
            lifetime.BracketButton(_characterRerollButton, () => characterRerollClicked?.Invoke());
            lifetime.BracketButton(_effectPurchaseButton, () => effectPurchaseClicked?.Invoke());
            lifetime.BracketButton(_effectRerollButton, () => effectRerollClicked?.Invoke());
        }

        public void SetRerollPrice(int price)
        {
            var text = $"{price}";
            _characterRerollPriceText.text = text;
            _effectRerollPriceText.text = text;
        }

        public void SetCharacterOffer(ShopOffer offer)
        {
            _characterPriceText.text = $"{offer.Price}";
            _characterCaseView.SetActive(true);
            _characterCaseView.SetIcon(offer.Icon);
            _characterCaseView.SetHealth(offer.Health, 0);
            _characterCaseView.SetDamage(offer.Damage, 0);
        }

        public void SetEffectOffer(ShopOffer offer)
        {
            _effectPriceText.text = $"{offer.Price}";
            _effectCaseView.gameObject.SetActive(true);
            _effectCaseView.SetIcon(offer.Icon);
            _effectCaseView.SetBackgroundColor(offer.BackgroundColor);
            _effectCaseView.SetEffectData(offer.EffectType, offer.EffectParameter, offer.Turns);
        }

        public void PlayCharacterRerollSuccess() => _characterRerollIcon.Play();
        public void PlayCharacterRerollFail() => _characterRerollIcon.PlayShakeRotation();
        public void PlayEffectRerollSuccess() => _effectRerollIcon.Play();
        public void PlayEffectRerollFail() => _effectRerollIcon.PlayShakeRotation();
    }
}