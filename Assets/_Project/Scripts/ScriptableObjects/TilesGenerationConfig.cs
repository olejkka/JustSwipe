using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/TilesGenerationConfig",
        fileName = "TilesGenerationConfig"
    )]
    public class TilesGenerationConfig : ScriptableObject
    {
        [field : Header("Размеры поля")]
        [field: SerializeField] public RectInt Bounds { get; private set; }
        
        [field : Header("Размеры центра поля")]
        [field: SerializeField] public RectInt CoreBounds { get; private set; }

        [field : Header("Шанс генерации обычной позиции")]
        [field : Range(0, 100)]
        [field : SerializeField] public int CommonGenerationChance { get; private set; }
        
        [field : Header("Шанс генерации центральной позиции")]
        [field : Range(0, 100)]
        [field : SerializeField] public int CoreGenerationChance { get; private set; }
    }
}