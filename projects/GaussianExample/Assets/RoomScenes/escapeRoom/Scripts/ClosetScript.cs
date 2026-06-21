using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetScript : MonoBehaviour
{
    public Rigidbody doorL;
    public Rigidbody doorR;

    public GameObject hiddenObject;

    private void Start()
    {
        doorL.transform.rotation = Quaternion.Euler(0, 90, 0);
        doorR.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void OpenDoors()
    {
        doorL.isKinematic = false;
        doorR.isKinematic = false;
        if (hiddenObject != null) hiddenObject.SetActive(true);
    }


    public void OnClosetGrabbed()
    {
        if (doorR.isKinematic) NarratorAudioManager.Instance.PlayClosetLocked();
    }
}
