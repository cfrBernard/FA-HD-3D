#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
#endif

#if UNITY_EDITOR
public class SettingsExporter : MonoBehaviour
{
    [MenuItem("Tools/Export All Settings Categories")]
    public static void ExportAllSettingsCategories()
    {
        SettingsData data = SettingsManager.Instance.GetSettingsData();

        ExportCategory("AudioSettingsCategory", data.audio, "Assets/AudioSettingsCategory.asset");
        ExportCategory("VideoSettingsCategory", data.video, "Assets/VideoSettingsCategory.asset");
        ExportCategory("GameplaySettingsCategory", data.gameplay, "Assets/GameplaySettingsCategory.asset");

        Debug.Log("[SettingsExporter] Exported all categories.");
    }

    private static void ExportCategory(string assetName, object settingsSection, string assetPath)
    {
        var category = ScriptableObject.CreateInstance<SettingsCategory>();
        category.parameters = new List<ParamDefinition>();
    
        string sectionName = assetName.Replace("SettingsCategory", "").ToLower(); // "audio", "video", etc.
    
        var fields = settingsSection.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    
        foreach (var field in fields)
        {
            ParamDefinition param = FieldToParam(field);
            if (param != null)
            {
                param.propertyPath = $"{sectionName}.{field.Name}";
                param.key = param.propertyPath; // << important que UI accède à la bonne clé
                param.label = ObjectNames.NicifyVariableName(field.Name);
                category.parameters.Add(param);
            }
        }
    
        AssetDatabase.CreateAsset(category, assetPath);
        AssetDatabase.SaveAssets();
        Debug.Log($"[SettingsExporter] Exported {assetName} to {assetPath}");
    }


    private static ParamDefinition FieldToParam(FieldInfo field)
    {
        ParamDefinition param = new ParamDefinition
        {
            key = field.Name,
            label = ObjectNames.NicifyVariableName(field.Name),
            showDecimal = false
        };

        if (field.FieldType == typeof(float))
        {
            param.type = ParamType.Slider;
            param.minValue = 0f;
            param.maxValue = 1f;
            param.showDecimal = true;
        }
        else if (field.FieldType == typeof(int))
        {
            param.type = ParamType.Slider;
            param.minValue = 0;
            param.maxValue = 10;
        }
        else if (field.FieldType == typeof(bool))
        {
            param.type = ParamType.Toggle;
        }
        else if (field.FieldType.IsEnum)
        {
            param.type = ParamType.Dropdown;
        }
        else
        {
            return null;
        }

        return param;
    }
}
#endif