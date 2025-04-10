using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutDirect : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        Color startColor = image.color;
        float timeElapsed = 0f;

        // Fade-out
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, timeElapsed / fadeDuration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        image.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        gameObject.SetActive(false);
    }
}
