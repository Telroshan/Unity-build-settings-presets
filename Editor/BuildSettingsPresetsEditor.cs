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
            Debug.Log("Initialization");
        }

        [MenuItem("Build presets/New")]
        private static void AddPreset()
        {
            Debug.Log("New preset !");
            BuildSettingsPreset preset = BuildSettingsPreset.FromCurrentSettings();
            Debug.Log(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(preset)));
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
        }
    }
}