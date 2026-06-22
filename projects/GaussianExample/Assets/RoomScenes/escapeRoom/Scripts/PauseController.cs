using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject pauseMenuCanvas;

    [Header("Input Action Reference")]
    [SerializeField] private InputActionProperty pauseAction;

    private bool isPaused = false;

    private void OnEnable()
    {
        // Enable the input action and subscribe to the perform event
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable to prevent memory leaks
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Activate or deactivate the UI menu element
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(isPaused);

        // Freeze or resume game physics and time
        Time.timeScale = isPaused ? 0f : 1f;
    }



    public void OnResumeClick()
    {
        TogglePause();
    }

    public void OnRestartClick()
    {
        TogglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitClick()
    {
        TogglePause();
        Application.Quit();
    }
}
