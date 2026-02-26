using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
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