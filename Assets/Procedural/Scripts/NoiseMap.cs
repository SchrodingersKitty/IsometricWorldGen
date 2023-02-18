using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Game.Procedural
{
    [CreateAssetMenu(menuName = "Data/NoiseMap")]
    public class NoiseMap : ScriptableObject
    {
        public enum NoiseFunction
        {
            Perlin,
            Simplex,
            Cellular
        }
        public NoiseFunction noiseFunction;
        public float noiseScale = 0.03f;
        public int octaves = 2;
        uint seed;
        Func<float2, float> func;
        float valueOffset;

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
                case NoiseFunction.Perlin:
                    func = noise.cnoise;
                    valueOffset = 0f;
                    break;
                case NoiseFunction.Simplex:
                    func = noise.snoise;
                    valueOffset = 0.5f;
                    break;
                case NoiseFunction.Cellular:
                    func = (p) => noise.cellular(p).x;
                    valueOffset = 0f;
                    break;
            }
        }

        public void SetSeed(uint seed)
        {
            this.seed = seed;
        }

        public float GetValue(Vector2Int pos)
        {
            float2 p = new float2(pos.x, pos.y) * noiseScale + (seed % 1000);
            float value = 0f;
            float maxValue = 0f;
            float frequency = 1f;
            float amplitude = 1f;
            for (int i = 0; i < octaves; i++)
            {
                value += (func(p * frequency) + valueOffset) * amplitude;
                maxValue += amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }
            value /= maxValue;
            return math.clamp(value, 0f, 1f);
        }
    }
}
