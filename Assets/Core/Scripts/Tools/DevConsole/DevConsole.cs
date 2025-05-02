using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DevConsole : MonoBehaviour
{
    public static DevConsole Instance { get; private set; }

    [Header("UI References")]
    public GameObject consoleUI;
    public TMP_InputField inputField;
    public RectTransform logContent;
    public ScrollRect scrollRect;
    public TMP_Text logText; 

    private List<string> commandHistory = new();
    private int historyIndex = -1;

    private bool showConsole = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            consoleUI.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleConsole();
        }

        if (!showConsole) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitCommand(inputField.text);
        }

        // Navigate history with arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (commandHistory.Count > 0)
            {
                historyIndex = Mathf.Clamp(historyIndex - 1, 0, commandHistory.Count - 1);
                inputField.text = commandHistory[historyIndex];
                inputField.MoveTextEnd(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (commandHistory.Count > 0)
            {
                historyIndex = Mathf.Clamp(historyIndex + 1, 0, commandHistory.Count - 1);
                inputField.text = commandHistory[historyIndex];
                inputField.MoveTextEnd(false);
            }
        }
    }

    private void ToggleConsole()
    {
        showConsole = !showConsole;
        consoleUI.SetActive(showConsole);

        if (showConsole)
        {
            inputField.ActivateInputField();
        }
    }

    private void SubmitCommand(string cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd)) return;

        commandHistory.Add(cmd);
        historyIndex = commandHistory.Count;
        inputField.text = "";
        inputField.ActivateInputField();

        Log($"> {cmd}");
        RunCommand(cmd);
    }

    private void RunCommand(string cmd)
    {
        if (cmd == "help")
        {
            Log("Available commands: help, echo [text]");
        }
        else if (cmd.StartsWith("echo "))
        {
            Log(cmd.Substring(5));
        }
        else
        {
            Log($"Unknown command: {cmd}");
        }
    }

    public void Log(string message)
    {
        logText.text += message + "\n";

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
