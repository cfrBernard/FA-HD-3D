using UnityEngine;

public class LoadingUIRotation : MonoBehaviour
{
    public RectTransform rectTransform;
    public float maxRotationSpeed = 100f;
    public float rotationRange = 180f;
    private float targetRotation;
    private float currentRotation;
    private float pulseSpeed;

    void Start()
    {
        if (rectTransform == null)
        {
            Debug.LogError("[LoadingUIRotation] No RectTransform !");
            return;
        }
        targetRotation = rectTransform.eulerAngles.y;
    }

    void Update()
    {
        float angleDifference = Mathf.DeltaAngle(currentRotation, targetRotation);
        pulseSpeed = Mathf.Lerp(10f, maxRotationSpeed, Mathf.Abs(angleDifference) / rotationRange);

        currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, pulseSpeed * Time.deltaTime);
        rectTransform.rotation = Quaternion.Euler(0, currentRotation, 0);

        if (Mathf.Approximately(currentRotation, targetRotation))
        {
            targetRotation = (targetRotation + 180f) % 360f;
        }
    }
}
