using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FlashlightScript : MonoBehaviour
{
    private Light light_;
    public bool isPowered;

    private XRGrabInteractable xrGI;

    public GameObject hiddenObject;

    private void Awake()
    {
        xrGI = GetComponentInChildren<XRGrabInteractable>();
        light_ = GetComponentInChildren<Light>();
    }

    void Start()
    {
        isPowered = false;
        light_.enabled = false;
    }

    private void OnEnable()
    {
        xrGI.activated.AddListener(OnActivated);
    }
    private void OnDisable()
    {
        xrGI.activated.RemoveListener(OnActivated);
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (isPowered)
            toggleLight();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPowered) return;
        if (collision.rigidbody && collision.rigidbody.CompareTag("Battery"))
        {
            isPowered = true;
            toggleLight();

            Destroy(collision.rigidbody.gameObject);

            hiddenObject.GetComponent<HiddenZ>().TriggerRequirement();

            NarratorAudioManager.Instance.PlayBatteriesInserted();
        }
    }


    private void toggleLight()
    {
        light_.enabled = !light_.enabled;
    }
}
