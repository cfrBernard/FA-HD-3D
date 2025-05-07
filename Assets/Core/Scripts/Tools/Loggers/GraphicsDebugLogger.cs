using UnityEngine;
using System.IO;
using System.Text;

public class GraphicsDebugLogger : MonoBehaviour
{
    void Start()
    {
        LogGraphicsInfo();
    }

    void LogGraphicsInfo()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== GRAPHICS & SYSTEM INFO ===");
        sb.AppendLine($"Unity Version: {Application.unityVersion}");
        sb.AppendLine($"Platform: {Application.platform}");
        sb.AppendLine($"Is Editor: {Application.isEditor}");
        sb.AppendLine($"Is Playing: {Application.isPlaying}");

        sb.AppendLine("\n--- Screen Info ---");
        sb.AppendLine($"Resolution: {Screen.currentResolution.width} x {Screen.currentResolution.height} @ {Screen.currentResolution.refreshRateRatio}Hz");
        sb.AppendLine($"Fullscreen: {Screen.fullScreen}");
        sb.AppendLine($"DPI: {Screen.dpi}");

        sb.AppendLine("\n--- System Info ---");
        sb.AppendLine($"Operating System: {SystemInfo.operatingSystem}");
        sb.AppendLine($"Processor: {SystemInfo.processorType} ({SystemInfo.processorCount} cores)");
        sb.AppendLine($"System Memory: {SystemInfo.systemMemorySize} MB");
        sb.AppendLine($"Device Model: {SystemInfo.deviceModel}");

        sb.AppendLine("\n--- Graphics Device Info ---");
        sb.AppendLine($"Device Name: {SystemInfo.graphicsDeviceName}");
        sb.AppendLine($"Device Type: {SystemInfo.graphicsDeviceType}");
        sb.AppendLine($"Device Vendor: {SystemInfo.graphicsDeviceVendor}");
        sb.AppendLine($"Device Version: {SystemInfo.graphicsDeviceVersion}");
        sb.AppendLine($"Graphics Memory: {SystemInfo.graphicsMemorySize} MB");
        sb.AppendLine($"Shader Level: {SystemInfo.graphicsShaderLevel}");
        sb.AppendLine($"Supports Compute Shaders: {SystemInfo.supportsComputeShaders}");
        sb.AppendLine($"Supports Instancing: {SystemInfo.supportsInstancing}");
        sb.AppendLine($"Supported Render Target Count: {SystemInfo.supportedRenderTargetCount}");
        sb.AppendLine($"Supports 2D Array Textures: {SystemInfo.supports2DArrayTextures}");
        sb.AppendLine($"Supports 3D Textures: {SystemInfo.supports3DTextures}");

        sb.AppendLine("\n--- Quality Settings ---");
        sb.AppendLine($"Quality Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
        sb.AppendLine($"VSync Count: {QualitySettings.vSyncCount}");
        sb.AppendLine($"Anti Aliasing: {QualitySettings.antiAliasing}");
        sb.AppendLine($"Anisotropic Filtering: {QualitySettings.anisotropicFiltering}");
        sb.AppendLine($"Soft Particles: {QualitySettings.softParticles}");
        sb.AppendLine($"Realtime Reflection Probes: {QualitySettings.realtimeReflectionProbes}");

        sb.AppendLine("\n--- Application Info ---");
        sb.AppendLine($"App Version: {Application.version}");
        sb.AppendLine($"Is Debug Build: {Debug.isDebugBuild}");
        sb.AppendLine($"Install Mode: {Application.installMode}");
        sb.AppendLine($"Internet Reachability: {Application.internetReachability}");
        sb.AppendLine($"Timestamp: {System.DateTime.Now}");

        string path = Path.Combine(Application.persistentDataPath, "GraphicsDebugLog.txt");
        File.WriteAllText(path, sb.ToString());

        Debug.Log($"[GraphicsDebugLogger] Graphics debug info written to: {path}");
    }
}
