using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDrawerScipt : MonoBehaviour
{
    private Rigidbody rb;
    private ConfigurableJoint joint;

    public GameObject hiddenObject;

    [SerializeField] private Transform otherDrawer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
    }

    void Start()
    {
        rb.isKinematic = true;
        transform.position -= transform.forward * joint.linearLimit.limit;

        otherDrawer.position -= transform.forward * joint.linearLimit.limit;


        Collider[] myColliders = GetComponentsInChildren<Collider>();
        Collider[] otherColliders = otherDrawer.GetComponentsInChildren<Collider>();
        foreach (var myCol in myColliders)
            foreach (var otherCol in otherColliders)
                Physics.IgnoreCollision(myCol, otherCol);
    }

    public void OpenDrawer()
    {
        rb.isKinematic = false;
        hiddenObject.SetActive(true);
    }


    public void OnDrawerGrabbed()
    {
        if (rb.isKinematic) NarratorAudioManager.Instance.PlayDrawerLocked();
    }
}
