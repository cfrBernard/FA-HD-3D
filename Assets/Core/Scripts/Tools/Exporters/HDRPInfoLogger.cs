using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

public class HDRPInfoLogger : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "HDRP_Info.txt");

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            
            LogQualitySettings(writer);
#if UNITY_HDRP
            LogHDRPSettings(writer);
            LogDefaultVolume(writer);
#else
            writer.WriteLine("HDRP is not enabled in this project.");
#endif
        }

        Debug.Log($"HDRP Info exported to: {filePath}");
    }



    void LogQualitySettings(StreamWriter writer)
    {
        writer.WriteLine("== Quality Settings ==");
        writer.WriteLine($"Current Quality Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
        writer.WriteLine($"AntiAliasing: {QualitySettings.antiAliasing}");
        writer.WriteLine($"Shadow Resolution: {QualitySettings.shadowResolution}");
        writer.WriteLine($"Shadow Distance: {QualitySettings.shadowDistance}");
        writer.WriteLine();
    }

#if UNITY_HDRP
    void LogHDRPSettings(StreamWriter writer)
    {
        writer.WriteLine("== HDRP Global Settings ==");

        var hdrpAsset = GraphicsSettings.renderPipelineAsset as HDRenderPipelineAsset;
        if (hdrpAsset != null)
        {
            writer.WriteLine($"Render Pipeline Asset: {hdrpAsset.name}");

            var hdrpSettings = hdrpAsset.currentPlatformRenderPipelineSettings;

            writer.WriteLine($"Support SSR: {hdrpSettings.supportSSR}");
            writer.WriteLine($"Support SSAO: {hdrpSettings.supportSSAO}");
            writer.WriteLine($"Support Subsurface Scattering: {hdrpSettings.supportSubsurfaceScattering}");
            writer.WriteLine($"Support Volumetrics: {hdrpSettings.supportVolumetrics}");
            writer.WriteLine($"Support Motion Vectors: {hdrpSettings.supportMotionVectors}");
            writer.WriteLine($"Support DBuffer: {hdrpSettings.supportDecals}");

            writer.WriteLine($"Max Shadow Distance: {hdrpSettings.shadowInitParams.maxShadowDistance}");
        }
        else
        {
            writer.WriteLine("HDRP Asset not found.");
        }

        writer.WriteLine();
    }

    void LogDefaultVolume(StreamWriter writer)
    {
        writer.WriteLine("== Default Volume Settings ==");

        var volumes = FindObjectsOfType<Volume>();
        foreach (var volume in volumes)
        {
            if (volume.isGlobal)
            {
                writer.WriteLine($"Found Global Volume: {volume.name}");
                foreach (var comp in volume.profile.components)
                {
                    writer.WriteLine($"Volume Override: {comp.name}");
                }

                writer.WriteLine();
                return;
            }
        }

        writer.WriteLine("No Global Volume found.");
        writer.WriteLine();
    }
#endif
}
