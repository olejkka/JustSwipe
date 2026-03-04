using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay/GameplayEconomyConfig",
        fileName = "GameplayEconomyConfig"
    )]
    public class GameplayEconomyConfig : ScriptableObject
    {
        public int RerollPrice;
    }
}