using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerActionController : MonoBehaviour
{
    public MoveObject moveObjectScript;          // Reference to the MoveObject script
    public GameObject targetObject;              // The GameObject to activate
    public AudioClip audioClip90Seconds;         // Audio clip to play when timer reaches 90 seconds
    public AudioClip audioClip30Seconds;         // Audio clip to play when timer reaches 30 seconds
    public AudioSource audioSource;              // AudioSource to play the audio clips

    public ParticleSystem[] particleSystems;     // Array to hold your particle systems
    public float startRate = 100f;               // Starting rate over time for particles
    public float endRate = 1000f;                // Ending rate over time for particles

    private bool played90SecondClip = false;
    private bool played30SecondClip = false;
    private float totalTimerDuration;

    void Start()
    {
        // Capture the initial timer value from the MoveObject script (120 in this case)
        totalTimerDuration = moveObjectScript.timer;
    }

    void Update()
    {
        // Check the timer in the MoveObject script
        float timer = moveObjectScript.timer;

        // Gradually increase particle emission rates over the entire timer (from start to 0)
        float normalizedTime = Mathf.InverseLerp(totalTimerDuration, 0f, timer);
        AdjustParticleEmission(normalizedTime);

        // When the timer reaches 90 seconds
        if (timer <= 90 && !played90SecondClip)
        {
            PlayAudioAndActivateObject(audioClip90Seconds);
            played90SecondClip = true;
        }

        // When the timer reaches 30 seconds
        if (timer <= 30 && !played30SecondClip)
        {
            PlayAudioAndActivateObject(audioClip30Seconds);
            played30SecondClip = true;
        }
    }

    // Method to play the audio clip and activate the target object
    void PlayAudioAndActivateObject(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);  // Play the audio clip
        }

        if (targetObject != null)
        {
            targetObject.SetActive(true);  // Activate the target object
        }
    }

    // Method to adjust particle emission gradually over the whole timer duration
    void AdjustParticleEmission(float normalizedTime)
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            var emission = ps.emission;
            // Gradually increase from 100 to 1000 based on normalized time
            emission.rateOverTime = Mathf.Lerp(startRate, endRate, normalizedTime);
        }
    }
}
