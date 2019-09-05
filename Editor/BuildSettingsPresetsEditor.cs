using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private static string GetAssetDirectory(BuildSettingsPreset preset = null)
        {
            if (preset == null)
            {
                preset = ScriptableObject.CreateInstance<BuildSettingsPreset>();
            }

            return Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(preset))) ??
                   ROOT_FOLDER;
        }

        private static void RefreshPresetsList()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(BuildSettingsPreset));
            Dictionary<string, string> presets = new Dictionary<string, string>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildSettingsPreset preset = AssetDatabase.LoadAssetAtPath<BuildSettingsPreset>(path);
                presets.Add(guid, preset.name);
            }

            if (!presets.Except(BuildSettingsPresetsMenuItems.presets).Any()
                && !BuildSettingsPresetsMenuItems.presets.Except(presets).Any())
            {
                return;
            }

            string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), GetAssetDirectory(),
                nameof(BuildSettingsPresetsMenuItems) + ".cs");

            StreamReader reading = File.OpenText(scriptPath);
            string str;
            bool inGeneratedSection = false;
            string newFileContent = "";
            while ((str = reading.ReadLine()) != null)
            {
                newFileContent += str + "\n";
                if (str.Trim() == "#region GeneratedPresets")
                {
                    inGeneratedSection = true;
                    break;
                }
            }

            foreach (KeyValuePair<string, string> entry in presets)
            {
                string presetGuid = entry.Key;
                string presetName = Regex.Replace(entry.Value, @"[()]", "");
                newFileContent += "\t\t\t{ \"" + presetGuid + "\", \"" + presetName + "\" },\n";
            }

            while ((str = reading.ReadLine()) != null)
            {
                if (str.Trim() == "#endregion")
                {
                    newFileContent += str + "\n";
                    inGeneratedSection = false;
                    break;
                }
            }

            while ((str = reading.ReadLine()) != null)
            {
                newFileContent += str + "\n";
                if (str.Trim() == "#region GeneratedMenuItems")
                {
                    inGeneratedSection = true;
                    break;
                }
            }

            foreach (KeyValuePair<string, string> entry in presets)
            {
                string presetGuid = entry.Key;
                string presetName = Regex.Replace(entry.Value, @"[()]", "");
                newFileContent += "\t\t[MenuItem(\"Build presets/ > " + presetName + "\")]\n" +
                                  "\t\tpublic static void Import" + presetGuid + "()\n" +
                                  "\t\t{\n" +
                                  "\t\t\tBuildSettingsPresetsEditor.ImportPreset(\"" + presetGuid + "\");\n" +
                                  "\t\t}\n";
            }

            while ((str = reading.ReadLine()) != null)
            {
                if (inGeneratedSection && str.Trim() == "#endregion")
                {
                    inGeneratedSection = false;
                }

                if (!inGeneratedSection)
                {
                    newFileContent += str + "\n";
                }
            }

            reading.Close();
            File.WriteAllText(scriptPath, newFileContent);
        }

        [MenuItem("Build presets/+ Create new preset")]
        private static void AddPreset()
        {
            BuildSettingsPreset preset = BuildSettingsPreset.FromCurrentSettings();
            string dirname = GetAssetDirectory(preset);
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

            Selection.activeObject = preset;

            RefreshPresetsList();
        }

        public static void ImportPreset(string presetGuid)
        {
        }
    }
}