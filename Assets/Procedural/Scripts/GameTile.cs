namespace Game
{
    public class GameTile
    {
        public enum TileType
        {
            Dirt,
            Grass,
            Rock,
            Sand,
            Water,
            Snow,
            Ice,
            Lava,
            GrassDry,
            Slime,
            Sandstone,
            Stone,
            Brick,
            Clay,
            Paved,
            Bridge
        }
        public TileType tileType;
        public bool elevated;
    }
}
