using UnityEditor;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public class BuildSettingsPresetsEditor
    {
        static BuildSettingsPresetsEditor()
        {
            Debug.Log("Initialization");
        }

        [MenuItem("Build presets/New")]
        private static void AddPreset()
        {
            Debug.Log("New preset !");
        }
    }
}
