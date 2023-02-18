using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Game.Procedural;
using Game.Tilemaps;
using Random = Unity.Mathematics.Random;

namespace Game
{
    [RequireComponent(typeof(Tilemap))]
    public class WorldMapManager : MonoBehaviour
    {
        Tilemap tilemap;
        public WorldMap worldMap;
        [Min(1)]
        public int generationSpeed;
        public TileAsset[] tiles;

        void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            var rng = new Random();
            rng.InitState((uint)System.DateTime.Now.ToFileTimeUtc());
            worldMap.SetSeed(rng.NextUInt());
        }

        void Start()
        {
            GenerateMap();
        }

        [ContextMenu("Generate")]
        void GenerateMap()
        {
            tilemap.ClearAllTiles();
            StartCoroutine(FillTilemap());
        }

        IEnumerable<Vector2Int> GetSpiralCoordinates()
        {
            int radius = 0;
            int x = 0;
            int y = 0;
            int direction = -1;
            yield return new Vector2Int(x, y);
            while (true)
            {
                for (int i = 0; i < radius; i++)
                {
                    x += direction;
                    yield return new Vector2Int(x, y);
                }
                for (int i = 0; i < radius; i++)
                {
                    y += direction;
                    yield return new Vector2Int(x, y);
                }
                direction *= -1;
                radius++;
            }
        }

        IEnumerator FillTilemap()
        {
            int n = 0;
            foreach(var pos in GetSpiralCoordinates())
            {
                var tile = worldMap.GetTile(pos);
                var tileAsset = tiles
                    .Where(v => v.tileType == tile.tileType)
                    .First(v => v.elevated == tile.elevated);
                tilemap.SetTile((Vector3Int)pos, tileAsset);
                n++;
                if(n % generationSpeed == 0) yield return null;
            }
        }
    }
}
