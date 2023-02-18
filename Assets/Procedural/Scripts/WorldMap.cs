using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Game.Procedural
{
    [CreateAssetMenu(menuName = "Data/WorldMap")]
    public class WorldMap : ScriptableObject
    {
        Dictionary<Vector2Int, int> tileData;
        public NoiseMap heightMap;
        public NoiseMap temperatureMap;
        public NoiseMap vegetationMap;
        public NoiseMap waterMap;
        public NoiseMap dangerMap;
        Random rng;

        public void SetSeed(uint seed)
        {
            rng = new Random(seed);
            heightMap.SetSeed(rng.NextUInt());
            temperatureMap.SetSeed(rng.NextUInt());
            vegetationMap.SetSeed(rng.NextUInt());
            waterMap.SetSeed(rng.NextUInt());
            dangerMap.SetSeed(rng.NextUInt());
        }

        public GameTile GetTile(Vector2Int pos)
        {
            var tile = new GameTile();
            float height = heightMap.GetValue(pos);
            float temperature = temperatureMap.GetValue(pos);
            float vegetation = vegetationMap.GetValue(pos);
            float water = waterMap.GetValue(pos);
            float danger = dangerMap.GetValue(pos);

            tile.elevated = height > 0.5f;

            if(vegetation < 0.05f)
            {
                tile.tileType = GameTile.TileType.Rock;
            }
            else if(vegetation < 0.3f)
            {
                tile.tileType = GameTile.TileType.Dirt;
            }
            else
            {
                tile.tileType = GameTile.TileType.Grass;
            }

            if(temperature < 0.1f)
            {
                tile.tileType = GameTile.TileType.Snow;
            }
            else if(temperature > 0.7f)
            {
                if(tile.tileType == GameTile.TileType.Dirt)
                {
                    tile.tileType = GameTile.TileType.Sand;
                }
                else if(tile.tileType == GameTile.TileType.Grass)
                {
                    tile.tileType = GameTile.TileType.GrassDry;
                }
            }

            if(water > 0.6f)
            {
                tile.tileType = GameTile.TileType.Water;
            }

            if(danger > 0.8f)
            {
                if(tile.tileType == GameTile.TileType.Snow)
                {
                    tile.tileType = GameTile.TileType.Ice;
                }
                else if(tile.tileType == GameTile.TileType.Sand)
                {
                    tile.tileType = GameTile.TileType.Lava;
                }
                else if(tile.tileType == GameTile.TileType.Water)
                {
                    tile.tileType = GameTile.TileType.Slime;
                }
            }

            return tile;
        }
    }
}
