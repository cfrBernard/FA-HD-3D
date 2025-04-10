using UnityEngine;
using System.Collections.Generic;

public class DevConsole : MonoBehaviour
{
    public static DevConsole Instance { get; private set; }
    public KeyCode toggleKey = KeyCode.BackQuote;
    private bool showConsole = false;
    private string input = "";
    private List<string> commandHistory = new List<string>();
    private Vector2 scrollPosition;

    // Paramètres de personnalisation pour le style
    private Color backgroundColor = new Color(0f, 0f, 0f, 0.5f);
    private Color bottomPanelColor = new Color(0f, 0f, 0f, 1f);

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

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            showConsole = !showConsole;

        // Ajouter à l'historique si une commande a été exécutée
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(input))
        {
            RunCommand(input);
            input = "";
        }
    }

    void OnGUI()
    {
        if (!showConsole) return;

        // Fond semi-opaque 
        GUI.color = backgroundColor; // La couleur de fond, avec opacité
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none); // Panneau global transparent

        // Panneau opaque (en bas de l'écran)
        GUI.color = bottomPanelColor; // Couleur avec opacité pour le panneau du bas
        GUI.Box(new Rect(0, Screen.height - Screen.height / 2 + 100, Screen.width, Screen.height / 2), GUIContent.none); // Panneau opaque en bas

        // affichage pour l'historique
        GUILayout.BeginArea(new Rect(20, Screen.height - Screen.height / 2 + 110, Screen.width - 20, Screen.height / 2 - 40)); // Espace pour l'historique
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(Screen.height / 2 - 160));

        foreach (string command in commandHistory)
        {
            GUI.color = Color.white;
            GUILayout.Label(command); // Affiche chaque commande de l'historique
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        // Champ d'entrée pour les commandes 
        GUI.color = new Color(0f, 0f, 0f, 1f); // Une couleur de fond sombre avec de l'opacité pour l'input
        GUILayout.BeginArea(new Rect(0, Screen.height - 30, Screen.width, 30)); // Zone de l'input collée en bas
        GUI.color = Color.white;
        input = GUILayout.TextField(input, GUILayout.Height(30));
        GUILayout.EndArea();
    }

    void RunCommand(string cmd)
    {
        commandHistory.Add(">" + cmd);

        // Commandes spécifiques
        if (cmd.ToLower() == "fps")
        {
            Debug.Log("FPS command executed!");
        }
        else if (cmd.ToLower() == "reload")
        {
            Debug.Log("Reloading scene...");
        }
        else
        {
            Debug.LogWarning("Unknown command: " + cmd);
        }

        // Si l'historique est trop long, on enlève la première commande pour ne pas dépasser une taille donnée
        if (commandHistory.Count > 10)
        {
            commandHistory.RemoveAt(0);
        }

        // Force le défilement vers le bas après chaque commande
        scrollPosition = new Vector2(0, Mathf.Infinity); // La position de scroll est mise à la fin de la liste
    }
}
