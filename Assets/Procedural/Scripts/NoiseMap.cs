using System;
using UnityEngine;
using Unity.Mathematics;

namespace Game.Procedural
{
    [CreateAssetMenu(menuName = "Data/NoiseMap")]
    public class NoiseMap : ScriptableObject
    {
        public enum NoiseFunction
        {
            White,
            Perlin,
            Simplex,
            CellularF1,
            CellularF2
        }
        public NoiseFunction noiseFunction;
        public float noiseScale = 0.03f;
        public float noiseMultiplier = 1f;
        [Min(1)]
        public int octaves = 2;
        uint seed;
        Func<float2, float> func;

        void Awake()
        {
            Initialize();
        }

        void OnValidate()
        {
            Initialize();
        }

        void Initialize()
        {
            switch (noiseFunction)
            {
                case NoiseFunction.White:
                    func = WhiteNoise;
                    break;
                case NoiseFunction.Perlin:
                    func = Perlin01;
                    break;
                case NoiseFunction.Simplex:
                    func = Simplex01;
                    break;
                case NoiseFunction.CellularF1:
                    func = CellularF1;
                    break;
                case NoiseFunction.CellularF2:
                    func = CellularF2;
                    break;
            }
        }

        public void SetSeed(uint seed)
        {
            this.seed = seed;
        }

        float WhiteNoise(float2 p)
        {
            float hash = math.sin(math.dot(p, new float2(12.9898f, 78.233f)));
            return math.frac(hash * 43758.5453f);
        }

        float Perlin01(float2 p) => noise.cnoise(p) * 0.5f + 0.5f;

        float Simplex01(float2 p) => noise.snoise(p) * 0.5f + 0.5f;

        float CellularF1(float2 p) => noise.cellular(p).x;

        float CellularF2(float2 p) => noise.cellular(p).y;

        public float GetValue(Vector2Int pos)
        {
            float2 p = new float2(pos.x, pos.y) * noiseScale + (seed % 1000);
            float value = 0f;
            float maxValue = 0f;
            float frequency = 1f;
            float amplitude = 1f;
            for (int i = 0; i < octaves; i++)
            {
                value += func(p * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }
            value = (value / maxValue - 0.5f) * noiseMultiplier + 0.5f;
            return math.clamp(value, 0f, 1f);
        }
    }
}
