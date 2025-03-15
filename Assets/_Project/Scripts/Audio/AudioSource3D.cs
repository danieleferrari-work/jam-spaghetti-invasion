using System.Collections;
using UnityEngine;

public class AudioSource3D : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;

    [Range(0f, 1f)]
    [SerializeField]
    float volumeVariance = .1f;

    [Range(0f, 1f)]
    [SerializeField]
    float pitchVariance = .1f;

    AudioSource audioSource;
    float halfVolumeVariance;
    float halfPitchVariance;

    void Start()
    {
        halfVolumeVariance = volumeVariance * .5f;
        halfPitchVariance = pitchVariance * .5f;

        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("No clips!", gameObject);
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        audioSource.clip = clips[randomIndex];

        SetVolumeAndPitch();

        audioSource.Play();
    }

    public void PlayFadeIn(float fadeDuration)
    {
        SetVolumeAndPitch();
        StartCoroutine(FadeIn(fadeDuration));
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void StopFadeOut(float fadeDuration)
    {
        SetVolumeAndPitch();
        StartCoroutine(FadeOut(fadeDuration));
    }


    IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.001)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;

            yield return null;
        }
        audioSource.Stop();
    }


    IEnumerator FadeIn(float fadeDuration)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime * fadeDuration;

            yield return null;
        }
    }

    void SetVolumeAndPitch()
    {
        audioSource.volume = audioSource.volume * (1f + Random.Range(-halfVolumeVariance, halfVolumeVariance));
        audioSource.pitch = audioSource.pitch * (1f + Random.Range(-halfPitchVariance, halfPitchVariance));
    }
}
