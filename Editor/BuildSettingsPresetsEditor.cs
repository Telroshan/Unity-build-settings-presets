using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class BuildSettingsPresetsEditor
    {
        private const string ROOT_FOLDER = "Assets";
        private const string DEFAULT_NAME = "NewPreset";
        
        static BuildSettingsPresetsEditor()
        {
            RefreshPresetsList();
        }

        [MenuItem("Build presets/Refresh")]
        private static void RefreshPresetsList()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(BuildSettingsPreset));
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildSettingsPreset preset = AssetDatabase.LoadAssetAtPath<BuildSettingsPreset>(path);
            }
        }

        [MenuItem("Build presets/New")]
        private static void AddPreset()
        {
            BuildSettingsPreset preset = BuildSettingsPreset.FromCurrentSettings();
            string dirname =
                Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(preset))) ??
                ROOT_FOLDER;
            if (dirname != ROOT_FOLDER)
            {
                dirname = Path.Combine(Directory.GetParent(dirname).ToString(), "Presets");
            }

            // Don't overwrite an existing preset
            string path;
            int suffix = 0;
            do
            {
                string fileName = suffix == 0
                    ? DEFAULT_NAME
                    : DEFAULT_NAME + "(" + suffix + ")";
                path = Path.Combine(dirname, fileName + ".asset");
                ++suffix;
            } while (File.Exists(path));

            AssetDatabase.CreateAsset(preset, path);
            
            RefreshPresetsList();
        }
    }
}