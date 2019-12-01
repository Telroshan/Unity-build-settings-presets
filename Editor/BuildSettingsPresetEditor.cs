using UnityEditor;
using UnityEngine;

namespace TelroshanTools.BuildSettingsPresets.Editor
{
    [CustomEditor(typeof(BuildSettingsPreset))]
    public class BuildSettingsPresetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Apply settings"))
            {
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(target, out string guid, out long _);
                BuildSettingsPresetsManager.ApplyPreset(guid);
            }
            
            if (GUILayout.Button("Overwrite with current build settings"))
            {
                ((BuildSettingsPreset)target).OverwriteWithCurrentBuildSettings();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            DrawDefaultInspector();
        }
    }
}