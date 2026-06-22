using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    [SerializeField] private string menuSceneName;
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }



    public Text timeText;

    private void Start()
    {
        if (GameTimer.Instance != null)
        {
            float finalTime = GameTimer.Instance.GetTime();

            // Format time into minutes and seconds
            int minutes = Mathf.FloorToInt(finalTime / 60F);
            int seconds = Mathf.FloorToInt(finalTime % 60F);

            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
