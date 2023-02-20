using UnityEngine;
using UnityEditor;
using Game.Procedural;

namespace Game.Procedural
{
    [CustomPreview(typeof(NoiseMap))]
    public class NoiseMapPreview : ObjectPreview
    {
        bool debug = false;
        Texture2D[] previewImages;

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override void Initialize(Object[] targets)
        {
            base.Initialize(targets);
            if(targets.Length == 0) return;
            previewImages = new Texture2D[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                previewImages[i] = new Texture2D(100, 100, TextureFormat.RGB24, false);
                previewImages[i].filterMode = FilterMode.Point;
            }
            RefreshAll();
        }

        public override void OnPreviewSettings()
        {
            if(GUILayout.Button("Debug"))
            {
                debug = true;
                RefreshAll();
            }
            if(GUILayout.Button("Refresh"))
            {
                debug = false;
                RefreshAll();
            }
        }

        void RefreshAll()
        {
            for (int i = 0; i < m_Targets.Length; i++)
            {
                FillPreview(previewImages[i], (NoiseMap)m_Targets[i]);
            }
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            GUI.DrawTexture(r, previewImages[m_ReferenceTargetIndex], ScaleMode.ScaleToFit);
        }

        void FillPreview(Texture2D image, NoiseMap map)
        {
            var array = new Color32[image.width * image.height];
            for (int x = 0; x < image.width; x++)
            {
                for (int y = 0; y < image.height; y++)
                {
                    int i = x * image.width + y;
                    float value = map.GetValue(new Vector2Int(x, y));
                    var color = new Color(value, value, value);
                    if(debug)
                    {
                        if(value == 0f) color = Color.blue;
                        if(value == 0.5f) color = Color.green;
                        if(value == 1f) color = Color.red;
                    }
                    array[i] = color;
                }
            }
            image.SetPixels32(array);
            image.Apply();
        }

        public override void Cleanup()
        {
            if(previewImages != null)
            {
                foreach(var img in previewImages)
                {
                    Object.DestroyImmediate(img);
                }
                previewImages = null;
            }
            base.Cleanup();
        }
    }
}
