using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public float elapsedTime = 0f;
    private bool isTimerRunning = false;
    private bool isTimerPaused = false;

    private void Awake()
    {
        // Keep this object alive across scenes
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

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (isTimerRunning && !isTimerPaused)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}
