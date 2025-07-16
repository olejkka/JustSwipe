using UnityEngine;
using UnityEngine.Tilemaps;
using _Project.Scripts.ScriptableObjects;
using VContainer;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Tilemap))]
    public class BoardGenerator : MonoBehaviour
    {
        private TileFieldConfig _config;
        private Tilemap _tilemap;
        
        
        [Inject]
        public void Construct(TileFieldConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            GenerateRandomRoom();
        }

        private void GenerateRandomRoom()
        {
            if (_config == null || _tilemap == null)
            {
                Debug.LogError("BoardGenerator: не назначен config или Tilemap!");
                return;
            }

            _tilemap.ClearAllTiles();

            int W = _config.Width;
            int H = _config.Height;

            int roomW = Random.Range(3, W + 1);
            int roomH = Random.Range(3, H + 1);
            int x0 = Random.Range(0, W - roomW + 1);
            int y0 = Random.Range(0, H - roomH + 1);

            for (int x = 0; x < W; x++)
            for (int y = 0; y < H; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                // базовый флаг «внутри комнаты»
                bool inside = x >= x0 && x < x0 + roomW
                           && y >= y0 && y < y0 + roomH;

                bool isGround = inside;

                // с некоторой вероятностью перекрываем базовую форму шумом
                if (Random.value < _config.Randomness)
                {
                    // решаем случайно, земля или препятствие
                    isGround = Random.value < _config.GroundProbability;
                }

                var type = isGround ? TileType.Ground : TileType.Obstacle;
                var tile = _config.GetTile(type);
                if (tile != null)
                    _tilemap.SetTile(cell, tile);
            }
        }
    }
}
