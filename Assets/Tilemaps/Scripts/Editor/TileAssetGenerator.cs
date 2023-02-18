using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Game.Tilemaps;

namespace Game.EditorTools
{
    public class TileAssetGenerator : EditorWindow
    {
        ObjectField objectField;
        Label infoLabel;

        [MenuItem("Tools/Tile Asset Generator")]
        public static void CreateWindow()
        {
            var wnd = GetWindow<TileAssetGenerator>(false, "Tile Asset Generator", true);
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            objectField = new ObjectField("Sprite sheet")
            {
                objectType = typeof(Texture2D),
                allowSceneObjects = false
            };
            objectField.RegisterValueChangedCallback(OnValueChanged);
            infoLabel = new Label();
            var button = new Button(Generate)
            {
                text = "Generate"
            };
            root.Add(objectField);
            root.Add(infoLabel);
            root.Add(button);
        }

        IEnumerable<Sprite> GetSprites(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            return assets.OfType<Sprite>();
        }

        void OnValueChanged(ChangeEvent<Object> e)
        {
            if(e.newValue == null)
            {
                infoLabel.text = "";
            }
            else
            {
                var sprites = GetSprites(e.newValue);
                infoLabel.text = sprites.Count() + " sprites";
            }
        }

        void Generate()
        {
            var sprites = GetSprites(objectField.value);

            var destination = "Assets";
            destination = EditorUtility.SaveFolderPanel("Save tiles to...", destination, "");
            if(string.IsNullOrEmpty(destination)) return;
            destination = Path.GetRelativePath(Path.Combine(Application.dataPath, ".."), destination);

            float n = sprites.Count();
            float i = 0f;
            foreach(var s in sprites)
            {
                var fullPath = Path.Combine(destination, s.name + ".asset");
                EditorUtility.DisplayProgressBar("Creating tiles...", fullPath, i / n);
                var tile = CreateInstance<TileAsset>();
                tile.m_DefaultSprite = s;
                tile.m_DefaultColliderType = UnityEngine.Tilemaps.Tile.ColliderType.Sprite;
                AssetDatabase.DeleteAsset(fullPath);
                AssetDatabase.CreateAsset(tile, fullPath);
                i++;
            }
            EditorUtility.ClearProgressBar();
        }
    }
}
