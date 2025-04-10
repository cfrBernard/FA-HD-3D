using UnityEngine;

public class DevConsole : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.BackQuote;
    private bool showConsole = false;
    private string input = "";

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showConsole = !showConsole;
    }

    void OnGUI()
    {
        if (!showConsole) return;

        GUILayout.BeginArea(new Rect(10, 10, 500, 100));
        GUILayout.Label("Console:");
        input = GUILayout.TextField(input);

        if (Event.current.keyCode == KeyCode.Return)
        {
            RunCommand(input);
            input = "";
        }

        GUILayout.EndArea();
    }

    void RunCommand(string cmd)
    {
        // commandes ici
    }
}
