using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputConfig", menuName = "Config/Input Config")]
public class InputConfig : ScriptableObject
{
    public InputActionAsset inputActions;
}