using UnityEngine;
using UnityEngine.InputSystem;

public class CompositeBindingDebugger : MonoBehaviour
{
    public InputActionAsset inputActions;

    void Start()
    {
        var actionMap = inputActions.FindActionMap("Player");
        var moveAction = actionMap.FindAction("Move");

        var bindings = moveAction.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];
            if (binding.isPartOfComposite)
            {
                string direction = binding.name;
                string key = InputControlPath.ToHumanReadableString
                (   
                    binding.effectivePath, 
                    InputControlPath.
                    HumanReadableStringOptions.
                    OmitDevice
                );
                Debug.Log($"{direction} = {key}");
            }
        }
    }
}
