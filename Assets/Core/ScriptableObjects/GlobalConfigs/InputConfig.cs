using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputConfig", menuName = "GlobalConfigs/Input Config")]
public class InputConfig : ScriptableObject
{
    public InputActionAsset inputActions;
}