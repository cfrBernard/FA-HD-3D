using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParamSlider : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TextMeshProUGUI valueText;
    public Slider slider;

    private bool showDecimal = false;

    public void Setup(
        string labelText,
        float initialValue,
        System.Action<float> onValueChanged,
        bool showDecimalValue = false,
        float minValue = 0f,
        float maxValue = 1f)
    {
        label.text = labelText;
        showDecimal = showDecimalValue;

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = initialValue;

        UpdateValueText(initialValue);

        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(value =>
        {
            UpdateValueText(value);
            onValueChanged(value);
        });
    }

    private void UpdateValueText(float value)
    {
        valueText.text = showDecimal ? value.ToString("0.0") : Mathf.RoundToInt(value).ToString();
    }
}
