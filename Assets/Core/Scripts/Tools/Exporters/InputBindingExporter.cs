#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class InputBindingExporter
{
    [MenuItem("Tools/Export Player Input Bindings")]
    public static void ExportPlayerBindings()
    {
        var config = Resources.Load<SettingsConfig>("GlobalConfigsSO/SettingsConfig");

        if (config == null)
        {
            Debug.LogError("[InputBindingExporter] SettingsConfig not found in Resources.");
            return;
        }

        var inputAsset = GlobalConfigs.Input.inputActions;
        if (inputAsset == null)
        {
            Debug.LogError("[InputBindingExporter] InputActionAsset is null in GlobalConfigs.");
            return;
        }

        string bindingsJson = "";
        var playerActionMap = inputAsset.FindActionMap("Player");

        if (playerActionMap == null)
        {
            Debug.LogError("[InputBindingExporter] Player ActionMap not found in InputActionAsset.");
            return;
        }

        // actionMap Player
        foreach (var action in playerActionMap.actions)
        {
            bindingsJson += $"Action: {action.name} - Bindings:\n";
            foreach (var binding in action.bindings)
            {
                bindingsJson += $"- {binding.path}\n";
            }
        }

        // Sauvegarde les bindings
        config.defaultInputBindings = new InputBindings
        {
            inputActionOverridesJson = bindingsJson
        };

        // Sauvegarde de l'asset modifié
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[InputBindingExporter] Player bindings exported into SettingsConfig.");
    }
}
#endif
