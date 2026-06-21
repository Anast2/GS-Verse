using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentScript : MonoBehaviour
{
    public int screwsCount = 4;

    private Rigidbody ventRb;
    private int removedScrews = 0;

    public GameObject safetyCollider;

    public GameObject hiddenObject;

    private void Start()
    {
        ventRb = GetComponent<Rigidbody>();

        if (ventRb != null)
            ventRb.isKinematic = true;
    }

    public void ScrewRemoved()
    {
        removedScrews++;

        if (removedScrews >= screwsCount)
        {
            if (ventRb != null)
                open();
        }
    }

    private void open()
    {
        ventRb.isKinematic = false;
        safetyCollider.SetActive(false);
        NarratorAudioManager.Instance.PlayVentOpened();
        hiddenObject.GetComponent<HiddenZ>().TriggerRequirement();
    }
}
