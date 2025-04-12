using UnityEngine;

public static class GlobalConfigs
{
    private static InputConfig _inputConfig;
    // private static AudioConfig _audioConfig;

    public static InputConfig Input => _inputConfig ??= Load<InputConfig>("GlobalConfigsSO/InputConfig");
    // public static AudioConfig Audio => _audioConfig ??= Load<AudioConfig>("AudioConfig");

    private static T Load<T>(string path) where T : ScriptableObject
    {
        var asset = Resources.Load<T>(path);
        if (asset == null)
            Debug.LogError($"[GlobalConfigs] Missing config at Resources/{path}");

        return asset;
    }
}

