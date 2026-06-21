using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int breakThreshold = 15;

    private int hitCount = 0;

    private BoxFace[] faces;

    [SerializeField]
    private GameObject safetyCollider;

    public GameObject[] hiddenObjects;

    [Header("Audio Setup")]
    public AudioSource audioSource;
    public AudioClip[] hitClips;
    public AudioClip breakClip;

    private void Start()
    {
        faces = GetComponentsInChildren<BoxFace>();
    }

    public void GetHit()
    {
        playHitSound();

        hitCount++;
        Debug.Log(hitCount);

        if(hitCount >= breakThreshold)
            Break();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.tag == "Hammer")
        {
            foreach (var face in faces)
            {
                face.Reload();
            }
        }
    }


    private void Break()
    {
        playBreakSound();

        safetyCollider.SetActive(false);
        foreach (var face in faces)
        {
            face.Break();
        }
        foreach (var obj in hiddenObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = false;
        }
    }



    private void playHitSound()
    {
        // Pick a random index between 0 and the total number of clips
        int randomIndex = Random.Range(0, hitClips.Length);

        // play the chosen clip
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hitClips[randomIndex]);
    }

    private void playBreakSound()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(breakClip);
    }
}
