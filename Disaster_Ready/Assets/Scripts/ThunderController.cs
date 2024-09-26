using System.Collections;
using UnityEngine;

public class ThunderController : MonoBehaviour
{
    public Light lightningLight;
    public float minTimeBetweenFlashes = 5f;
    public float maxTimeBetweenFlashes = 15f;
    public float lightningDuration = 0.2f;

    private AudioSource thunderSound;

    void Start()
    {
        // Turn off lightning light by default
        lightningLight.intensity = 0f;
        thunderSound = GetComponent<AudioSource>();

        // Start the coroutine to control the lightning and thunder timing
        StartCoroutine(LightningEffect());
    }

    IEnumerator LightningEffect()
    {
        while (true)
        {
            // Random wait before the next lightning flash
            float waitTime = Random.Range(minTimeBetweenFlashes, maxTimeBetweenFlashes);
            yield return new WaitForSeconds(waitTime);

            // Trigger lightning flash
            StartCoroutine(TriggerLightningFlash());
        }
    }

    IEnumerator TriggerLightningFlash()
    {
        // Define an array of four specific large numbers
        float[] flashIntensities = { 1000f, 3000f, 5000f, 7000f };

        // Choose a random number from the array
        float flashIntensity = flashIntensities[Random.Range(0, flashIntensities.Length)];


        // Lightning appears
        lightningLight.intensity = flashIntensity;

        // Play thunder sound slightly delayed for realism
        float thunderDelay = Random.Range(0.5f, 1.5f); // Thunder typically follows lightning
        Invoke("PlayThunderSound", thunderDelay);

        // Lightning stays for a brief duration
        yield return new WaitForSeconds(lightningDuration);

        // Fade lightning out
        while (lightningLight.intensity > 0)
        {
            lightningLight.intensity -= Time.deltaTime * flashIntensity;
            yield return null;
        }
    }

    void PlayThunderSound()
    {
        if (thunderSound != null)
        {
            thunderSound.Play();
        }
    }
}
