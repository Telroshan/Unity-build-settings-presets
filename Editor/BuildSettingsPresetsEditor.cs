using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Dictionary<string, string> presets = new Dictionary<string, string>();
            Debug.Log(guids.Length + " presets found");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildSettingsPreset preset = AssetDatabase.LoadAssetAtPath<BuildSettingsPreset>(path);
                presets.Add(guid, preset.name);
            }

            if (!presets.Except(BuildSettingsPresetsMenuItems.presets).Any()
            && !BuildSettingsPresetsMenuItems.presets.Except(presets).Any())
            {
                Debug.Log("No diff");
                return;
            }
            
            Debug.Log("Difference found, regenerating file...");
            BuildSettingsPresetsMenuItems.presets = presets;
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