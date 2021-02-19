using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TelroshanTools.BuildSettingsPresets.Editor
{
    [InitializeOnLoad]
    public class BuildSettingsPresetsManager : AssetPostprocessor
    {
        private const string RootFolder = "Assets";
        private const string DefaultName = "NewPreset";

        #region Presets actions

        [MenuItem("Build presets/+ New (from current settings)")]
        private static void AddPreset()
        {
            BuildSettingsPreset preset = BuildSettingsPreset.FromCurrentSettings();
            string dirname = GetCurrentDirectory(preset);
            if (dirname != RootFolder)
            {
                dirname = Path.Combine(Directory.GetParent(dirname).ToString(), "Presets");
            }

            // Don't overwrite an existing preset
            string path;
            int suffix = 0;
            do
            {
                string fileName = suffix == 0
                    ? DefaultName
                    : DefaultName + "(" + suffix + ")";
                path = Path.Combine(dirname, fileName + ".asset");
                ++suffix;
            } while (File.Exists(path));

            AssetDatabase.CreateAsset(preset, path);

            Selection.activeObject = preset;

            RefreshPresetsList();
        }

        public static void ApplyPreset(string presetGuid)
        {
            AssetDatabase.LoadAssetAtPath<BuildSettingsPreset>(AssetDatabase.GUIDToAssetPath(presetGuid)).Apply();

            UpdateCsProj();

            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        #endregion

        #region Code generation

        public static void RefreshPresetsList()
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
            if (!presets.Except(BuildSettingsPresetsMenuItems.Presets).Any()
                && !BuildSettingsPresetsMenuItems.Presets.Except(presets).Any())
            {
                return;
            }

            string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), GetCurrentDirectory(),
                nameof(BuildSettingsPresetsMenuItems) + ".cs");

            StreamReader reader = File.OpenText(scriptPath);
            string newFileContent = "";

            string presetsGenerated = String.Join("", presets.Select(entry =>
            {
                string presetGuid = entry.Key;
                string presetName = entry.Value;
                return "\t\t\t{\"" + presetGuid + "\", \"" + presetName + "\"},\n";
            }));
            newFileContent = InsertInRegion(reader, newFileContent, presetsGenerated, "GeneratedPresets");

            string menuItemsGenerated = String.Join("\n", presets.Select(entry =>
            {
                string presetGuid = entry.Key;
                string presetName = entry.Value;
                return "\t\t[MenuItem(\"Build presets/" + presetName + "\")]\n" +
                       "\t\tpublic static void Apply" + presetGuid + "()\n" +
                       "\t\t{\n" +
                       "\t\t\t" + typeof(BuildSettingsPresetsManager).Name + "." + nameof(ApplyPreset) + "(\"" +
                       presetGuid + "\");\n" +
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

        /// <summary>
        /// I noticed with several tests, that sometimes Unity doesn't automatically update the DefineConstants
        /// in the csproj file... Resulting in wrong display in IDEs like Rider
        /// </summary>
        private static void UpdateCsProj()
        {
            string csProjPath = Path.Combine(Directory.GetCurrentDirectory(), "Assembly-CSharp.csproj");
            if (!File.Exists(csProjPath)) return;

            string csProjContent = File.ReadAllText(csProjPath);
            string tag = "DefineConstants";
            string openTag = $"<{tag}>";
            int startIndex = csProjContent.IndexOf(openTag, StringComparison.Ordinal) + openTag.Length;
            int length = csProjContent.IndexOf($"</{tag}", StringComparison.Ordinal) - startIndex;

            string savedDefineConstants = csProjContent.Substring(startIndex, length);

            string currentDefineConstants = string.Join(";", PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';')
                .Concat(EditorUserBuildSettings.activeScriptCompilationDefines)
                .Distinct());

            csProjContent = csProjContent.Replace(savedDefineConstants, currentDefineConstants);
            File.WriteAllText(csProjPath, csProjContent);
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
                    currentContent += "\n";
                    break;
                }
            }

            currentContent += contentToInsert;

            while ((str = reader.ReadLine()) != null)
            {
                if (str.Trim() == "#endregion")
                {
                    currentContent += "\n" + str + "\n";
                    break;
                }
            }

            return currentContent;
        }

        #endregion

        private static string GetCurrentDirectory(BuildSettingsPreset preset = null)
        {
            if (preset == null)
            {
                preset = ScriptableObject.CreateInstance<BuildSettingsPreset>();
            }

            return Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(preset))) ??
                   RootFolder;
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            RefreshPresetsList();
        }
    }

    public class BuildSettingsPresetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(sourcePath);
            if (asset.GetType() == typeof(BuildSettingsPreset))
            {
                // A preset changed (renamed or moved), refresh list
                // Don't refresh immediately because this function gets called right before the asset gets moved
                EditorApplication.delayCall += RefreshDelayCall;
            }

            return AssetMoveResult.DidNotMove;
        }

        private static void RefreshDelayCall()
        {
            BuildSettingsPresetsManager.RefreshPresetsList();
            EditorApplication.delayCall -= RefreshDelayCall;
        }
    }
}