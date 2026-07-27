using _Project.Scripts.Characters.Structs___Enums;
using UnityEngine;

namespace _Project.Scripts.GameplayEconomy
{
    public enum ShopOfferType
    {
        None = 0,
        Character = 1,
        Effect = 2
    }

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
        public EffectType EffectType;
        public int EffectParameter;
        public int Turns;
    }
}