using System.Collections;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;

    private IEnumerator Start()
    {
        Debug.Log("SceneFader Start");

        fadeCanvas.alpha = 1f;

        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        fadeCanvas.alpha = 0f;
        NarratorAudioManager.Instance.PlayIntro();
    }
}