using UnityEditor;
using UnityEngine;

namespace BuildSettingsPresets
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
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            DrawDefaultInspector();
        }
    }
}