using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class ParamDropdown : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TMP_Dropdown dropdown;

    public void Setup(string labelText, string[] options, int currentIndex, Action<int> onValueChanged)
    {
        label.text = labelText;
        dropdown.ClearOptions();
        dropdown.AddOptions(options.ToList());
        dropdown.value = currentIndex;
        dropdown.onValueChanged.AddListener(onValueChanged.Invoke);
    }
}
