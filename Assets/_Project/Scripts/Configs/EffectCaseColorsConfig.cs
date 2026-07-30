using System;
using _Project.Scripts.Characters.Effects;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Effect Case Colors",
        fileName = "Effect Case Colors"
    )]
    public class EffectCaseColorsConfig : ScriptableObject
    {
        [SerializeField] private Color _buffBackgroundColor;
        [SerializeField] private Color _debuffBackgroundColor;


        public Color GetBackgroundColor(EffectPolarity polarity)
        {
            return polarity switch
            {
                EffectPolarity.Buff => _buffBackgroundColor,
                EffectPolarity.Debuff => _debuffBackgroundColor,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(polarity),
                    polarity,
                    null)
            };
        }
    }
}