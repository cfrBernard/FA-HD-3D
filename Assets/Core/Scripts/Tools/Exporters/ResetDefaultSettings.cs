#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

#if UNITY_EDITOR
public class ResetDefaultSettings
{
    [MenuItem("Tools/Settings/Reset & Export Fresh JSON")]
    public static void ResetAndSaveDefaultSettings()
    {
        var freshData = new SettingsData().CopyFrom(GlobalConfigs.Settings);
        SaveManager.SaveSettings(freshData);

        Debug.Log("[Tools] New settings.json.");
    }

}
#endif