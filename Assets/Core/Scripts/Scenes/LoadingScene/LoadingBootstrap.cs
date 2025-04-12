using UnityEngine;
using System.Collections;

public class LoadingBootstrap : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        yield return FadeOut();
        yield return new WaitForSeconds(0.5f);
        string target = SceneManager.Instance.GetTargetSceneToLoad();
        yield return SceneManager.Instance.LoadSceneAsync(target, true);
        yield return FadeIn();
        yield return new WaitForSeconds(0.5f);

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("LoadingScene");
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
    }
}
