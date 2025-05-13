using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParamUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image background;
    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.white;

    public void OnSelect(BaseEventData eventData)
    {
        if (background != null)
            background.color = activeColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (background != null)
            background.color = inactiveColor;
    }
}
