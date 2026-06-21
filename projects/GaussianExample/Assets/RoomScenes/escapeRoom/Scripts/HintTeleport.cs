using UnityEngine;
using System.Collections;

public class HintTeleport : MonoBehaviour
{
    [Header("XR Configuration")]
    [Tooltip("Assign your XR Origin root GameObject here.")]
    public Transform xrOriginTransform;

    [Header("UI Configuration")]
    public CanvasGroup hintCanvasGroup;
    public float fadeDuration = 1.0f;
    public float delayBeforeHint = 15.0f;

    [Tooltip("Minimum distance in meters the player must move to count as a teleport.")]
    public float teleportDistanceThreshold = 0.1f;

    private bool hasTeleported = false;
    private Vector3 startingPosition;
    private Coroutine currentFadeRoutine;

    private void Start()
    {
        if (xrOriginTransform != null)
        {
            // Record where the player started the scene
            startingPosition = xrOriginTransform.position;
        }
        else
        {
            Debug.LogError("HintTeleport: Missing XR Origin Transform assignment!", this);
        }

        // Start the 15-second hint countdown
        StartCoroutine(StartHintTimer());
    }

    private void Update()
    {
        // Stop checking once we know they have successfully moved
        if (hasTeleported || xrOriginTransform == null) return;

        // Calculate the distance between current position and the start position
        float distanceMoved = Vector3.Distance(xrOriginTransform.position, startingPosition);

        // If they moved past the threshold, they must have teleported
        if (distanceMoved > teleportDistanceThreshold)
        {
            OnSuccessfulTeleport();
        }
    }

    private IEnumerator StartHintTimer()
    {
        yield return new WaitForSeconds(delayBeforeHint);

        if (!hasTeleported)
        {
            FadeHint(1f); // Fade In
        }
    }

    private void OnSuccessfulTeleport()
    {
        hasTeleported = true;
        FadeHint(0f); // Fade Out
    }

    private void FadeHint(float targetAlpha)
    {
        if (currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        if (hintCanvasGroup == null) yield break;

        float startAlpha = hintCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            hintCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        hintCanvasGroup.alpha = targetAlpha;
    }
}
