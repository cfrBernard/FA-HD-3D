using UnityEngine;

public class StartMenuRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationAmount = 5f;

    private Quaternion initialRotation;

    private void Start()
    {
        if (target != null)
            initialRotation = target.rotation;
    }

    private void Update()
    {
        if (target == null) return;

        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Position de la souris normalisée
        float x = (mousePos.x / screenWidth - 0.5f) * 2f;
        float y = (mousePos.y / screenHeight - 0.5f) * 2f;
        float yOffset = x * rotationAmount;
        float zOffset = -y * rotationAmount;

        Quaternion offsetRotation = Quaternion.Euler(0f, yOffset, zOffset);
        Quaternion targetRotation = initialRotation * offsetRotation;

        target.rotation = Quaternion.Slerp(target.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
