using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabContents;

    private int currentTabIndex = 0;

    private void Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // éviter problème de closure dans lambda
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }

        SwitchTab(currentTabIndex);
    }

    public void SwitchTab(int index)
    {
        foreach (var content in tabContents)
        {
            content.SetActive(false);
        }
        
        tabContents[index].SetActive(true);

        for (int i = 0; i < tabButtons.Length; i++)
        {
            var colorBlock = tabButtons[i].colors;
            colorBlock.normalColor = (i == index) ? Color.green : Color.white;
            tabButtons[i].colors = colorBlock;
        }

        currentTabIndex = index;
    }
}
