using System;
using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Character Case Colors",
        fileName = "Character Case Colors"
    )]
    public class CharacterCaseColorsConfig : ScriptableObject
    {
        [SerializeField] private Color _playerBackgroundColor;
        [SerializeField] private Color _botBackgroundColor;

        
        public Color GetBackgroundColor(Team team)
        {
            return team switch
            {
                Team.Player => _playerBackgroundColor,
                Team.Bot => _botBackgroundColor,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(team),
                    team,
                    null)
            };
        }
    }
}