using System.Reflection;
using UnityEngine;

public static class ReflectionUtils 
{
    public static object GetValueByPath(object obj, string path)
    {
        string[] parts = path.Split('.');
        object current = obj;

        foreach (var part in parts)
        {
            if (current == null) return null;
            var field = current.GetType().GetField(part, BindingFlags.Instance | BindingFlags.Public);
            if (field == null) return null;

            current = field.GetValue(current);
        }

        return current;
    }

    public static void SetValueByPath(object target, string path, object value)
    {
        string[] parts = path.Split('.');
        object current = target;

        for (int i = 0; i < parts.Length - 1; i++)
        {
            var field = current?.GetType().GetField(parts[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
            {
                Debug.LogError($"[ReflectionUtils] Field '{parts[i]}' not found in path '{path}'");
                return;
            }

            current = field.GetValue(current);
            if (current == null)
            {
                Debug.LogError($"[ReflectionUtils] Null intermediate object at '{parts[i]}' in path '{path}'");
                return;
            }
        }

        var targetField = current.GetType().GetField(parts[^1], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (targetField == null)
        {
            Debug.LogError($"[ReflectionUtils] Final field '{parts[^1]}' not found in path '{path}'");
            return;
        }

        targetField.SetValue(current, value);
    }
}