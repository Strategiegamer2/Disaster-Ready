using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector timelineDirector;   // Reference to the Timeline Director
    public ParticleSystem[] particleSystems;    // Array to hold your particle systems
    public float startRate = 10f;               // Starting rate over time for particles
    public float endRate = 100f;                // Ending rate over time for particles
    public float startEnvIntensity = 1f;        // Starting intensity multiplier of environment lighting
    public float endEnvIntensity = 3f;          // Ending intensity multiplier of environment lighting
    public string nextSceneName;                // Name of the next scene to load

    private float timelineDuration;

    void Start()
    {
        // Get the duration of the timeline
        timelineDuration = (float)timelineDirector.duration;

        // Initialize particles and environment lighting
        foreach (ParticleSystem ps in particleSystems)
        {
            var emission = ps.emission;
            emission.rateOverTime = startRate;
        }

        // Set the initial environment lighting intensity
        RenderSettings.ambientIntensity = startEnvIntensity;

        // Play the timeline
        timelineDirector.Play();
    }

    void Update()
    {
        if (timelineDirector.state == PlayState.Playing)
        {
            // Calculate the normalized time (0 to 1) based on the timeline progress
            float normalizedTime = (float)(timelineDirector.time / timelineDuration);

            // Gradually increase particle system emission rates
            foreach (ParticleSystem ps in particleSystems)
            {
                var emission = ps.emission;
                emission.rateOverTime = Mathf.Lerp(startRate, endRate, normalizedTime);
            }

            // Gradually change the environment lighting intensity
            RenderSettings.ambientIntensity = Mathf.Lerp(startEnvIntensity, endEnvIntensity, normalizedTime);

            // Check if the timeline is over
            if (timelineDirector.time >= timelineDuration)
            {
                Debug.Log("Werkt");
                ChangeScene();
            }
        }
    }

    // Method to change the scene when the timeline ends
    void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
