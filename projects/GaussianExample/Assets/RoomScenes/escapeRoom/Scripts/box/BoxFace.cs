using UnityEngine;
using GaussianSplatting.Shared;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxFace : MonoBehaviour
{
    private IDeformable deformer;

    public bool boxIsBroken = false;

    private Box box;

    private bool canBeHit = true;

    private void Awake()
    {
        deformer = GetComponent<IDeformable>();
        box = GetComponentInParent<Box>();
    }

    private void AddPressForce(Vector3 worldPoint, Vector3 normal)
    {
        if (boxIsBroken) return;
        deformer.AddPressForce(worldPoint, normal);
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody && collision.rigidbody.tag == "Hammer")
            getHit(collision);
    }

    private void getHit(Collision collision)
    {
        if (!canBeHit || boxIsBroken) return;

        ContactPoint contact = collision.GetContact(0);
        AddPressForce(contact.point, -contact.normal);

        canBeHit = false;

        box.GetHit();
    }



    public void Reload()
    {
        canBeHit = true;
    }

    public void Break()
    {
        boxIsBroken = true;
        GetComponent<Rigidbody>().isKinematic = false;
        //GetComponentInChildren<XRGrabInteractable>().colliders.Add(GetComponent<MeshCollider>());
    }
}