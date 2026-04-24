using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Gameplay Economy",
        fileName = "Gameplay Economy"
    )]
    public class GameplayEconomyConfig : ScriptableObject
    {
        public int RerollPrice;
    }
}