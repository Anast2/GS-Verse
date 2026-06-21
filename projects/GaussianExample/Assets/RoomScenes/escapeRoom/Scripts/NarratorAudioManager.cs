using System.Collections.Generic;
using UnityEngine;

public class NarratorAudioManager : MonoBehaviour
{
    public static NarratorAudioManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip intro;

    [SerializeField] private AudioClip flashlightPickup;
    [SerializeField] private AudioClip batteriesPickup;
    [SerializeField] private AudioClip batteriesInserted;
    [SerializeField] private AudioClip hammerPickup;
    [SerializeField] private AudioClip screwdriverPickup;

    [SerializeField] private AudioClip plantHintPaperPickup;
    [SerializeField] private AudioClip safePuzzlePaperPickup;

    [SerializeField] private AudioClip drawerLocked;
    [SerializeField] private AudioClip closetLocked;
    [SerializeField] private AudioClip doorLocked;

    [SerializeField] private AudioClip safeWrongCode1;
    [SerializeField] private AudioClip safeWrongCode2;
    [SerializeField] private AudioClip safeWrongCode3;

    [SerializeField] private AudioClip drawerUnlocked;
    [SerializeField] private AudioClip closetUnlocked;
    [SerializeField] private AudioClip safeOpened;
    [SerializeField] private AudioClip ventOpened;

    [SerializeField] private AudioClip finalDoorUnlocked;
    [SerializeField] private AudioClip ending;

    private readonly HashSet<string> playedEvents = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void TryPlay(string eventId, AudioClip clip)
    {
        if (clip == null) return;

        if (playedEvents.Contains(eventId))
            return;

        if (audioSource.isPlaying)
            return;

        playedEvents.Add(eventId);
        audioSource.clip = clip;
        audioSource.Play();
    }

    // ----------------------------------------------------
    // Public API
    // ----------------------------------------------------

    public void PlayIntro()
        => TryPlay(nameof(PlayIntro), intro);

    public void PlayFlashlightPickup()
        => TryPlay(nameof(PlayFlashlightPickup), flashlightPickup);

    public void PlayBatteriesPickup()
        => TryPlay(nameof(PlayBatteriesPickup), batteriesPickup);

    public void PlayBatteriesInserted()
        => TryPlay(nameof(PlayBatteriesInserted), batteriesInserted);

    public void PlayHammerPickup()
        => TryPlay(nameof(PlayHammerPickup), hammerPickup);

    public void PlayScrewdriverPickup()
        => TryPlay(nameof(PlayScrewdriverPickup), screwdriverPickup);

    public void PlayPlantHintPaperPickup()
        => TryPlay(nameof(PlayPlantHintPaperPickup), plantHintPaperPickup);

    public void PlaySafePuzzlePaperPickup()
        => TryPlay(nameof(PlaySafePuzzlePaperPickup), safePuzzlePaperPickup);

    public void PlayDrawerLocked()
        => TryPlay(nameof(PlayDrawerLocked), drawerLocked);

    public void PlayClosetLocked()
        => TryPlay(nameof(PlayClosetLocked), closetLocked);

    public void PlayDoorLocked()
        => TryPlay(nameof(PlayDoorLocked), doorLocked);


    private int safeWrongCodeCount = 0;
    public void PlaySafeWrongCode()
    {
        if (audioSource.isPlaying)
            return;

        switch (safeWrongCodeCount)
        {
            case 0:
                audioSource.PlayOneShot(safeWrongCode1);
                break;

            case 1:
                audioSource.PlayOneShot(safeWrongCode2);
                break;

            case 2:
                audioSource.PlayOneShot(safeWrongCode3);
                break;

            default:
                return; // no more narrator lines
        }

        safeWrongCodeCount++;
    }


    public void PlayDrawerUnlocked()
        => TryPlay(nameof(PlayDrawerUnlocked), drawerUnlocked);

    public void PlayClosetUnlocked()
        => TryPlay(nameof(PlayClosetUnlocked), closetUnlocked);

    public void PlaySafeOpened()
        => TryPlay(nameof(PlaySafeOpened), safeOpened);

    public void PlayVentOpened()
    => TryPlay(nameof(PlayVentOpened), ventOpened);

    public void PlayFinalDoorUnlocked()
        => TryPlay(nameof(PlayFinalDoorUnlocked), finalDoorUnlocked);

    public void PlayEnding()
        => TryPlay(nameof(PlayEnding), ending);
}