using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private List<Light> lights = new List<Light>();

    [Header("Flickering Settings")]
    [SerializeField] private float minFlickerInterval = 1f; // Tempo minimo tra un lampeggio e l'altro
    [SerializeField] private float maxFlickerInterval = 3f; // Tempo massimo tra un lampeggio e l'altro
    [SerializeField] private float flickerDuration = 0.2f; // Durata di ogni flicker
    [SerializeField] private float flickerProbability = 0.02f; // Probabilità che una luce lampeggi ogni intervallo

    [Header("Power Surge Settings")]
    [SerializeField] private float surgeProbability = 0.1f; // Probabilità di sbalzo
    [SerializeField] private float surgeIntensityFactor = 0.3f; // Percentuale di riduzione intensità
    [SerializeField] private float surgeDuration = 1.5f; // Tempo per il ripristino della luce

    private void Start()
    {
        // Trova tutte le luci figlie dell'oggetto "Lights"
        foreach (Transform child in transform)
        {
            Light light = child.GetComponentInChildren<Light>();
            if (light != null)
            {
                lights.Add(light);
            }
        }

        // Avvia il lampeggiamento e gli sbalzi di tensione
        foreach (Light light in lights)
        {
            StartCoroutine(FlickerLight(light));
            StartCoroutine(PowerSurge(light));
        }
    }

    private IEnumerator FlickerLight(Light light)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));

            if (Random.value < flickerProbability) 
            {
                light.enabled = false;
                yield return new WaitForSeconds(flickerDuration);
                light.enabled = true;
            }
        }
    }

    private IEnumerator PowerSurge(Light light)
    {
        float initialIntensity = light.intensity;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f)); // Tempo casuale tra uno sbalzo e l'altro

            if (Random.value < surgeProbability) 
            {
                float targetIntensity = initialIntensity * surgeIntensityFactor;
                float elapsedTime = 0f;

                // Diminuizione graduale
                while (elapsedTime < surgeDuration / 2)
                {
                    light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / (surgeDuration / 2));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                light.intensity = targetIntensity;
                yield return new WaitForSeconds(surgeDuration / 4);

                // Ripristino graduale
                elapsedTime = 0f;
                while (elapsedTime < surgeDuration / 2)
                {
                    light.intensity = Mathf.Lerp(targetIntensity, initialIntensity, elapsedTime / (surgeDuration / 2));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                light.intensity = initialIntensity;
            }
        }
    }
}
