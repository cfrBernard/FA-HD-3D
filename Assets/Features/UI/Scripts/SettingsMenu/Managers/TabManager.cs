using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TabManager : MonoBehaviour
{
    public Button[] tabButtons;
    public GameObject[] tabContents;

    private int currentTabIndex = 0;

    public InputAction nextTabAction;
    public InputAction previousTabAction;

    private _TabButton[] tabButtonScripts;

    private void OnEnable()
    {
        nextTabAction.Enable();
        previousTabAction.Enable();

        nextTabAction.performed += _ => SwitchToNextTab();
        previousTabAction.performed += _ => SwitchToPreviousTab();
    }

    private void OnDisable()
    {
        nextTabAction.performed -= _ => SwitchToNextTab();
        previousTabAction.performed -= _ => SwitchToPreviousTab();

        nextTabAction.Disable();
        previousTabAction.Disable();
    }

    private void Start()
    {
        tabButtonScripts = new _TabButton[tabButtons.Length];

        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
            tabButtonScripts[i] = tabButtons[i].GetComponent<_TabButton>();
        }

        SwitchTab(currentTabIndex);
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == index);
        }

        for (int i = 0; i < tabButtonScripts.Length; i++)
        {
            if (tabButtonScripts[i] != null)
                tabButtonScripts[i].SetActive(i == index);
        }

        currentTabIndex = index;

        // Focus un élément dans l'onglet
        // EventSystem.current.SetSelectedGameObject
    }

    private void SwitchToNextTab()
    {
        int nextIndex = (currentTabIndex + 1) % tabButtons.Length;
        SwitchTab(nextIndex);
    }

    private void SwitchToPreviousTab()
    {
        int prevIndex = (currentTabIndex - 1 + tabButtons.Length) % tabButtons.Length;
        SwitchTab(prevIndex);
    }
}
