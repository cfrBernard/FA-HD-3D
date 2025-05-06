using UnityEngine;
using UnityEngine.InputSystem;
using System.Text;
using System.IO;

public class InputDebugLogger : MonoBehaviour
{
    private InputActionAsset inputActions;

    private void Awake()
    {
        inputActions = GlobalConfigs.Input.inputActions;
    }

    void Start()
    {
        LogInputSystemInfo();
    }

    void LogInputSystemInfo()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== INPUT SYSTEM DEBUG INFO ===");
        sb.AppendLine($"Unity Version: {Application.unityVersion}");
        sb.AppendLine($"Input System Version: {InputSystem.version}");
        sb.AppendLine($"Active Control Scheme: {(PlayerInput.all.Count > 0 ? PlayerInput.all[0].currentControlScheme : "N/A")}");
        sb.AppendLine($"Platform: {Application.platform}");
        sb.AppendLine($"Is Editor: {Application.isEditor}");
        sb.AppendLine($"Is Playing: {Application.isPlaying}");

        sb.AppendLine("\n--- Connected Devices ---");
        foreach (var device in InputSystem.devices)
        {
            sb.AppendLine($"Device: {device.displayName} ({device.name})");
            sb.AppendLine($"Layout: {device.layout}, Usage: {string.Join(", ", device.usages)}");
            sb.AppendLine($"Device ID: {device.deviceId}, Added: {device.added}");
            sb.AppendLine();
        }
        sb.AppendLine();

        if (inputActions != null)
        {
            sb.AppendLine("--- InputActionAsset Bindings (you can't rebind here) ---");
            foreach (var map in inputActions.actionMaps)
            {
                sb.AppendLine($"\nAction Map: {map.name}");
                foreach (var action in map.actions)
                {
                    sb.AppendLine($"  Action: {action.name} ({action.type})");

                    foreach (var binding in action.bindings)
                    {
                        sb.AppendLine($"    Binding: {binding.effectivePath} | Interactions: {binding.interactions} | Processors: {binding.processors}");
                        if (!string.IsNullOrEmpty(binding.overridePath))
                        {
                            sb.AppendLine($"    ➤ OVERRIDE: {binding.overridePath}");
                        }
                    }
                }
            }

            // sb.AppendLine("\n--- Input Action Overrides JSON ---");
        }

        sb.AppendLine("\n--- Application Info ---");
        sb.AppendLine($"App Version: {Application.version}");
        sb.AppendLine($"Is Debug Build: {Debug.isDebugBuild}");
        sb.AppendLine($"Install Mode: {Application.installMode}");
        sb.AppendLine($"Internet Reachability: {Application.internetReachability}");
        sb.AppendLine($"Timestamp: {System.DateTime.Now}");

        string path = Path.Combine(Application.persistentDataPath, "InputDebugLog.txt");
        File.WriteAllText(path, sb.ToString());

        Debug.Log($"[InputDebugLogger] Infos input logged at: {path}");
    }
}
