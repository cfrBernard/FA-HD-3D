using UnityEngine;
using System.Collections;

public class PrebootLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        SceneManager.Instance.LoadScene("BootScene");
    }
}
