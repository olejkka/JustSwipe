using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Effects;
using UnityEngine;

namespace _Project.Scripts.GameplayEconomy
{
    public class ShopOffer
    {
        public ShopOfferType Type;
        public string DefinitionId;
        public int Price;
        public Sprite Icon;
        
        // Character payload
        public int Health;
        public int Damage;
        
        // Effect payload
        public Team TargetTeam;
        public EffectType EffectType;
        public int EffectParameter;
        public int Turns;
        public Color BackgroundColor;
    }
}