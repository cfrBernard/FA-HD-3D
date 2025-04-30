using UnityEngine;
using System.Collections.Generic;

public class DevConsole : MonoBehaviour
{
    public static DevConsole Instance { get; private set; }

    private List<string> commandHistory = new List<string>();
    private bool showConsole = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    private void RunCommand(string cmd)
    {

    }
}
