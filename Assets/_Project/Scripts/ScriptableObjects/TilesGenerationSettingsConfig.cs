using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/TilesGenerationSettingsConfig",
        fileName = "TilesGenerationSettingsConfig"
    )]
    public class TilesGenerationSettingsConfig : ScriptableObject
    {
        [Header("Диапазон размеров комнаты")]
        [Min(1)] [SerializeField] private int _minRoomWidth;
        [Min(1)] [SerializeField] private int _minRoomHeight;

        [Min(1)] [SerializeField] private int _maxRoomWidth;
        [Min(1)] [SerializeField] private int _maxRoomHeight;

        [Header("Настройки случайности")]
        [Range(0f, 1f)]
        [SerializeField] private float _randomness;

        public int MinRoomWidth => _minRoomWidth;
        public int MinRoomHeight => _minRoomHeight;
        public int MaxRoomWidth => _maxRoomWidth;
        public int MaxRoomHeight => _maxRoomHeight;
        public float Randomness => _randomness;
    }
}