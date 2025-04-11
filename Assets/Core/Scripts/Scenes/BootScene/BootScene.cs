using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BootScene : MonoBehaviour
{
    [Header("UI Elements")]
    public Image progressBar;
    public Image FadeInPanel;
    public float fadeDuration = 0.5f;

    // Progress Bar
    private float totalSteps = 2f;
    private float currentStep = 0f;
    private float progressSpeed = 0.3f;

    // Singletons
    private GameManager gameManager;
    private SceneManager sceneManager;
    private SettingsManager settingsManager;
    private InputManager inputManager;
    private ToolsManager toolsManager;
    private FPSDisplay _FPSDisplay;
    

    #region BootSequence
    private void Start()
    {
        StartCoroutine(BootSequence());
    }

    private IEnumerator BootSequence()
    {
        yield return new WaitForSeconds(0.5f);

        InitSingletons();
        UpdateProgressBar();
        // yield return StartCoroutine(LoadPlayerPrefs());
        // UpdateProgressBar();
        // yield return StartCoroutine(LoadStaticData());
        // UpdateProgressBar();
        // yield return StartCoroutine(InitBackendServices());
        // UpdateProgressBar();
        // yield return StartCoroutine(LoadUserProfile());
        // UpdateProgressBar();
        // yield return StartCoroutine(PrepareAssets());
        
        yield return new WaitForSeconds(0.5f); 
        UpdateProgressBar();
        
        yield return StartCoroutine(FadeInRoutine());
        yield return StartCoroutine(LoadMainMenuAsync());
    }

    private void InitSingletons()
    {
        if (GameManager.Instance == null)
        {
            gameManager = new GameObject("GameManager").AddComponent<GameManager>();
            DontDestroyOnLoad(gameManager.gameObject);
        }
        else
            gameManager = GameManager.Instance;
    
        if (SceneManager.Instance == null)
        {
            sceneManager = new GameObject("SceneManager").AddComponent<SceneManager>();
            DontDestroyOnLoad(sceneManager.gameObject);
        }
        else
            sceneManager = SceneManager.Instance;
    
        if (SettingsManager.Instance == null)
        {
            settingsManager = new GameObject("SettingsManager").AddComponent<SettingsManager>();
            DontDestroyOnLoad(settingsManager.gameObject);
        }
        else
            settingsManager = SettingsManager.Instance;
    
        if (InputManager.Instance == null)
        {
            inputManager = new GameObject("InputManager").AddComponent<InputManager>();
            DontDestroyOnLoad(inputManager.gameObject);
        }
        else
            inputManager = InputManager.Instance;

        if (ToolsManager.Instance == null)
        { 
            toolsManager = new GameObject("toolsManager").AddComponent<ToolsManager>();
            DontDestroyOnLoad(toolsManager.gameObject);
        }
        else 
            toolsManager = ToolsManager.Instance;

        if (FPSDisplay.Instance == null)
        {
            _FPSDisplay = new GameObject("_FPSDisplay").AddComponent<FPSDisplay>();
            DontDestroyOnLoad(_FPSDisplay.gameObject);
        }
        else
            _FPSDisplay = FPSDisplay.Instance;
    }

    // IEnumerator LoadPlayerPrefs() {}
    // IEnumerator LoadStaticData() {}
    // IEnumerator InitBackendServices() {}
    // IEnumerator LoadUserProfile() {}
    // IEnumerator PrepareAssets() {}
    
    private IEnumerator LoadMainMenuAsync()
    {
        yield return new WaitForSeconds(0.1f);
        
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
        
        yield return operation;
        Debug.Log("MainMenu loaded!");
    }
    #endregion

    #region UI
    private IEnumerator FadeInRoutine()
    {
        Color startColor = FadeInPanel.color;
        float timeElapsed = 0f;

        // Fade-in
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 1f, timeElapsed / fadeDuration);
            FadeInPanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        FadeInPanel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
    }

    private void UpdateProgressBar()
    {
        currentStep += 1f;
        StartCoroutine(SmoothProgressBarUpdate(progressBar.fillAmount, currentStep / totalSteps));
    }
    
    private IEnumerator SmoothProgressBarUpdate(float startValue, float targetValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < progressSpeed)
        {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = Mathf.Lerp(startValue, targetValue, elapsedTime / progressSpeed);
            yield return null;
        }
        progressBar.fillAmount = targetValue;
    }
    #endregion
}
