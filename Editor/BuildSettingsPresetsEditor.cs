﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class BuildSettingsPresetsEditor : AssetPostprocessor
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

        private static string InsertInRegion(StreamReader reader, string currentContent, string contentToInsert,
            string regionName)
        {
            string str;
            while ((str = reader.ReadLine()) != null)
            {
                currentContent += str + "\n";
                if (str.Trim() == "#region " + regionName)
                {
                    break;
                }
            }

            currentContent += contentToInsert;

            while ((str = reader.ReadLine()) != null)
            {
                if (str.Trim() == "#endregion")
                {
                    currentContent += str + "\n";
                    break;
                }
            }

            return currentContent;
        }

        private static void RefreshPresetsList()
        {
            // Get all the presets assets
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(BuildSettingsPreset));
            Dictionary<string, string> presets = new Dictionary<string, string>();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BuildSettingsPreset preset = AssetDatabase.LoadAssetAtPath<BuildSettingsPreset>(path);
                presets.Add(guid, preset.name);
            }

            // Don't refresh if nothing changed
            if (!presets.Except(BuildSettingsPresetsMenuItems.presets).Any()
                && !BuildSettingsPresetsMenuItems.presets.Except(presets).Any())
            {
                return;
            }

            string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), GetAssetDirectory(),
                nameof(BuildSettingsPresetsMenuItems) + ".cs");

            StreamReader reader = File.OpenText(scriptPath);
            string newFileContent = "";

            string presetsGenerated = String.Join("", presets.Select(entry =>
            {
                string presetGuid = entry.Key;
                string presetName = entry.Value;
                return "\t\t\t{ \"" + presetGuid + "\", \"" + presetName + "\" },\n";
            }));
            newFileContent = InsertInRegion(reader, newFileContent, presetsGenerated, "GeneratedPresets");

            string menuItemsGenerated = String.Join("", presets.Select(entry =>
            {
                string presetGuid = entry.Key;
                string presetName = entry.Value;
                return "\t\t[MenuItem(\"Build presets/ > " + presetName + "\")]\n" +
                       "\t\tpublic static void Import" + presetGuid + "()\n" +
                       "\t\t{\n" +
                       "\t\t\tBuildSettingsPresetsEditor.ImportPreset(\"" + presetGuid + "\");\n" +
                       "\t\t}\n";
            }));
            newFileContent = InsertInRegion(reader, newFileContent, menuItemsGenerated, "GeneratedMenuItems");

            // Read the rest of the file
            string str;
            while ((str = reader.ReadLine()) != null)
            {
                newFileContent += str + "\n";
            }

            reader.Close();
            File.WriteAllText(scriptPath, newFileContent);

            AssetDatabase.Refresh();
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

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            RefreshPresetsList();
        }
    }
}