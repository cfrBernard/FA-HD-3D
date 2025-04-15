using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // UI
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator TransitionToLoadingScene()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        
        yield return FadeOut();

        yield return new WaitForSeconds(1f);

        UIManager.Instance.OnPlayGame();
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.gameObject.SetActive(false);
    }

    // Buttons
    public void PlayGame()
    {
        StartCoroutine(TransitionToLoadingScene());
    }

    public void OpenSettings()
    {
        UIManager.Instance.OnOpenSettings();
    }

    public void ExitGame()
    {
        UIManager.Instance.OnExitGame();
    }
}
