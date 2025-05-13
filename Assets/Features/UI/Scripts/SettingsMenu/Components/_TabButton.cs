using UnityEngine;
using UnityEngine.UI;

public class _TabButton : MonoBehaviour
{
    public Image selectedBackground;

    public void SetActive(bool selected)
    {
        selectedBackground.enabled = selected;
    }
}

