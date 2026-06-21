using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewScript : MonoBehaviour
{
    private VentScript vent;

    private bool removed = false;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vent = GetComponentInParent<VentScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (removed)
            return;

        if (collision.rigidbody && collision.rigidbody.CompareTag("Screwdriver"))
        {
            removed = true;

            rb.isKinematic = false;

            vent.ScrewRemoved();
        }
    }

}
