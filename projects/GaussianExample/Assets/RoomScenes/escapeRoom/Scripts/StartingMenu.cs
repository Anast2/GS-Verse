using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private string gameSceneName = "GameScene";

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    private IEnumerator StartGameRoutine()
    {
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        SceneManager.LoadScene(gameSceneName);
    }
}