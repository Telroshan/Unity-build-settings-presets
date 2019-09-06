using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BuildSettingsPreset))]
    public class BuildSettingsPresetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Apply"))
            {
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(target, out string guid, out long localId);
                BuildSettingsPresetsManager.ApplyPreset(guid);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            DrawDefaultInspector();
        }
    }
}