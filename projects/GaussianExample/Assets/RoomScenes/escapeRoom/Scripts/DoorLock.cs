using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLock : MonoBehaviour
{
    public Rigidbody doorRB;

    public GameObject Key;
    public GameObject keyGrabPoint;


    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private string endSceneName;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Key")
        {
            Unlock();
        }
    }


    private void Unlock()
    {
        GameTimer.Instance.StopTimer();

        Destroy(keyGrabPoint);

        foreach (var coll in Key.GetComponentsInChildren<Collider>())
            Destroy(coll);

        Destroy(Key.GetComponent<Rigidbody>());

        Key.transform.SetParent(doorRB.transform, true);


        StartCoroutine(AnimateKey());
    }





    private IEnumerator AnimateKey()
    {
        // 1. Move key into position in front of lock
        yield return MoveAndRotate(
            new Vector3(-328.700012f, -174.5f, -956.400024f),
            Quaternion.Euler(270f, 0f, 0f),
            0.4f
        );

        // 2. Push key into lock
        yield return SlideIntoLock(
            new Vector3(-328.700012f, -93.5f, -956.400024f),
            0.25f
        );

        // 3. Turn key
        yield return RotateAndOpen(
            Quaternion.Euler(270f, 0f, -179f),
            0.3f
        );
    }

    private IEnumerator MoveAndRotate(Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = Key.transform.localPosition;
        Quaternion startRot = Key.transform.localRotation;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            Key.transform.localPosition =
                Vector3.Lerp(startPos, targetPos, p);

            Key.transform.localRotation =
                Quaternion.Slerp(startRot, targetRot, p);

            yield return null;
        }

        Key.transform.localPosition = targetPos;
        Key.transform.localRotation = targetRot;
    }

    private IEnumerator SlideIntoLock(Vector3 targetPos, float duration)
    {
        Vector3 startPos = Key.transform.localPosition;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            Key.transform.localPosition =
                Vector3.Lerp(startPos, targetPos, p);

            yield return null;
        }

        Key.transform.localPosition = targetPos;
    }

    private IEnumerator RotateAndOpen(Quaternion targetRot, float duration)
    {
        Quaternion startRot = Key.transform.localRotation;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            Key.transform.localRotation =
                Quaternion.Slerp(startRot, targetRot, p);

            yield return null;
        }

        Key.transform.localRotation = targetRot;


        // open door
        doorRB.isKinematic = false;
        doorRB.AddForceAtPosition(transform.up * 1.5f, transform.position, ForceMode.Impulse);
        NarratorAudioManager.Instance.PlayFinalDoorUnlocked();

        // wait 2 frames, to make sure AudioSource isPlaying
        yield return null; yield return null;

        // wait until narrator finishes and at least 5 seconds pass
        var audio = NarratorAudioManager.Instance.audioSource;
        t = 0f;
        while (audio != null && audio.isPlaying || t < 5f)
        {
            t += Time.deltaTime;
            yield return null;
        }


        // THE END
        StartCoroutine(EndGameRoutine());
    }


    private IEnumerator EndGameRoutine()
    {
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        SceneManager.LoadScene(endSceneName);
    }



    public void OnDoorGrabbed()
    {
        if (doorRB.isKinematic) NarratorAudioManager.Instance.PlayDoorLocked();
    }
}
