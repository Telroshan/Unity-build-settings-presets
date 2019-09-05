using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class BuildSettingsPresetsEditor
    {
        private const string ROOT_FOLDER = "Assets";

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
            string path = Path.Combine(dirname, "NewPreset.asset");
            
            AssetDatabase.CreateAsset(preset, path);
        }
    }
}