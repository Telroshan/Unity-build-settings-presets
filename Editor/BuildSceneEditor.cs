using UnityEditor;
using UnityEngine;

namespace TelroshanTools.BuildSettingsPresets.Editor
{
    [CustomPropertyDrawer(typeof(BuildSettingsPreset.BuildScene))]
    public class BuildSceneEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var enabledRect = new Rect(position.x, position.y, 30, position.height);
            var sceneRect = new Rect(position.x + 35, position.y, position.width - 40, position.height);

            // GUIContent.none to draw property without label
            EditorGUI.PropertyField(enabledRect,
                property.FindPropertyRelative(nameof(BuildSettingsPreset.BuildScene.enabled)), GUIContent.none);

            EditorGUI.BeginChangeCheck();
            SerializedProperty sceneProperty =
                property.FindPropertyRelative(nameof(BuildSettingsPreset.BuildScene.scene));
            EditorGUI.PropertyField(sceneRect, sceneProperty, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                string path = AssetDatabase.GetAssetPath(sceneProperty.objectReferenceValue);
                property.FindPropertyRelative(nameof(BuildSettingsPreset.BuildScene.path)).stringValue = path;
                property.FindPropertyRelative(nameof(BuildSettingsPreset.BuildScene.guid)).stringValue =
                    AssetDatabase.AssetPathToGUID(path);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}