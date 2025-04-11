using UnityEngine;
using System.Collections;

public class PrebootLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        GameManager.Instance.SetGameState(GameState.Boot);
        SceneManager.Instance.LoadScene(SceneNames.Boot);
    }
}
