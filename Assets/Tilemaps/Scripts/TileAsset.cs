using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tilemaps
{
    [CreateAssetMenu(menuName = "Data/Tile Asset")]
    public class TileAsset : IsometricRuleTile<TileAsset.Neighbor>
    {
        public GameTile.TileType tileType;
        public bool elevated;
        public bool hasRoad;
        public bool isWater;

        public class Neighbor : TilingRuleOutput.Neighbor
        {
            public const int IsBridgable = 3;
            public const int IsNotBridgable = 4;
        }

        bool IsEquivalent(TileBase other)
        {
            if (other is not TileAsset) return false;
            var tile = (TileAsset)other;
            if(tile.isWater) return tile == this;
            return tile.tileType == tileType || tile.hasRoad == hasRoad;
        }

        bool IsBridgable(TileBase other)
        {
            if (other is not TileAsset) return false;
            var tile = (TileAsset)other;
            return tile.tileType == GameTile.TileType.Water || tile.tileType == GameTile.TileType.Slime;
        }

        public override bool RuleMatch(int neighbor, TileBase other)
        {
            // Check equality with elevated variant if it exists
            switch (neighbor)
            {
                case Neighbor.This:
                    return IsEquivalent(other);
                case Neighbor.NotThis:
                    return !IsEquivalent(other);
                case Neighbor.IsBridgable:
                    return IsBridgable(other);
                case Neighbor.IsNotBridgable:
                    return !IsBridgable(other);
            }
            return true;
        }
    }
}
