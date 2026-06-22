using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Safe : MonoBehaviour
{
    public Transform SafeHandle;
    public Rigidbody SafeDoor;
    public GameObject grabPoint;

    private bool isAnimating;

    public float openForceMult = 1f;

    public SafeDigit d1;
    public SafeDigit d2;
    public SafeDigit d3;

    public int correctCode;

    private bool isOpen = false;

    public GameObject hiddenObject;

    public void TryOpen()
    {
        if (isOpen) return;

        if (isAnimating) return;

        if (d1.value * 100 + d2.value * 10 + d3.value == correctCode)
            open();
        else
            wrongCode();
    }

    private void open()
    {
        isOpen = true;
        SafeDoor.isKinematic = false;
        StartCoroutine(SpinHandle(720f, 1f)); // 2 full rotations
        grabPoint.SetActive(true);
        SafeHandle.GetComponent<XRSimpleInteractable>().enabled = false;
        SafeHandle.GetComponent<Collider>().enabled = false;
        hiddenObject.SetActive(true);
        NarratorAudioManager.Instance.PlaySafeOpened();
    }
    private void wrongCode()
    {
        StartCoroutine(WrongHandleAnimation());// 90 degree and back
        NarratorAudioManager.Instance.PlaySafeWrongCode();
    }






    private IEnumerator WrongHandleAnimation()
    {
        isAnimating = true;

        Quaternion startRot = SafeHandle.localRotation;
        Quaternion targetRot = startRot * Quaternion.Euler(90f, 0f, 0f);

        float duration = 0.1f;

        // Turn 90°
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SafeHandle.localRotation = Quaternion.Slerp(startRot, targetRot, t / duration);
            yield return null;
        }

        // Turn back
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            SafeHandle.localRotation = Quaternion.Slerp(targetRot, startRot, t / duration);
            yield return null;
        }

        SafeHandle.localRotation = startRot;
        isAnimating = false;
    }


    private IEnumerator SpinHandle(float degrees, float duration)
    {
        isAnimating = true;

        float startX = SafeHandle.localEulerAngles.x;
        float endX = startX + degrees;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float x = Mathf.Lerp(startX, endX, t / duration);

            Vector3 euler = SafeHandle.localEulerAngles;
            euler.x = x;
            SafeHandle.localEulerAngles = euler;

            yield return null;
        }

        Vector3 finalEuler = SafeHandle.localEulerAngles;
        finalEuler.x = endX;
        SafeHandle.localEulerAngles = finalEuler;


        SafeDoor.AddForceAtPosition(-SafeDoor.transform.right * openForceMult, SafeHandle.position, ForceMode.Impulse);

        isAnimating = false;
    }

}
