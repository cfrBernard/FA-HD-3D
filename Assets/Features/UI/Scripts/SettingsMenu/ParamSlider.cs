using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParamSlider : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TextMeshProUGUI valueText;
    public Slider slider;


    private bool showDecimal = false;

    public void Setup(string labelText, float initialValue, System.Action<float> onValueChanged, bool showDecimalValue = false)
    {
        label.text = labelText;
        slider.value = initialValue;
        showDecimal = showDecimalValue;

        UpdateValueText(initialValue);

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
