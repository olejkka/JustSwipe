using UnityEngine;
using UnityEngine.Tilemaps;
using _Project.Scripts.Factories.Interfaces;
using _Project.Scripts.ScriptableObjects;
using VContainer;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Tilemap))]
    public class BoardGenerator : MonoBehaviour
    {
        private TileFieldConfig _config;
        private ITileFactory _tileFactory;
        private Tilemap _tilemap;

        [Inject]
        public void Construct(TileFieldConfig config, ITileFactory tileFactory)
        {
            _config = config;
            _tileFactory = tileFactory;
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
            if (_config == null || _tilemap == null || _tileFactory == null)
            {
                Debug.LogError("BoardGenerator: не назначен config, tilemap или tileFactory!");
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
                bool inside = x >= x0 && x < x0 + roomW
                                      && y >= y0 && y < y0 + roomH;

                bool isGround = inside;
                if (Random.value < _config.Randomness)
                    isGround = Random.value < _config.GroundProbability;

                var type = isGround ? TileType.Ground : TileType.Obstacle;
                _tileFactory.CreateTile(new Vector2Int(x, y), type);
            }
        }
    }
}