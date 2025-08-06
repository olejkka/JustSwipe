using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts.Factories
{
    public class TileInstantiator : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilesPrefabsConfig _prefabsConfig;


        public void Instantiate(Vector2Int position)
        {
            var pos = new Vector3Int(position.x, position.y, 0);
            var tile = PickTile();
            _tilemap.SetTile(pos, tile);
        }

        private TileBase PickTile()
        {
            foreach (var entry in _prefabsConfig.Entries)
            {
                if (entry.TileType == TileType.Ground)
                    continue;
                if (Random.Range(0, 100) < entry.Chance)
                    return entry.TileAsset;
            }

            return _prefabsConfig.GetTile(TileType.Ground);
        }
    }
}