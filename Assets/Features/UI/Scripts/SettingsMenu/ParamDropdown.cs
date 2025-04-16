using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParamDropdown : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TMP_Dropdown dropdown;

    public void Setup(string labelText, string[] options, int currentIndex, System.Action<int> onValueChanged)
    {
        label.text = labelText;
        dropdown.ClearOptions();
        dropdown.AddOptions(new System.Collections.Generic.List<string>(options));
        dropdown.value = currentIndex;
        dropdown.onValueChanged.AddListener(onValueChanged.Invoke);
    }
}
