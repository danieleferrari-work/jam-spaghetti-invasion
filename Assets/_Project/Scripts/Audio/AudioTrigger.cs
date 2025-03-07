using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    private AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerGondola"))
        {
            //AudioManager.instance.Play("testClips");
            Sound sound = AudioManager.instance.GetSoundByName("testClips");

            if (sound != null && audioSource != null)
            {
                audioSource.clip = sound.source.clip;
                audioSource.volume = sound.source.volume;
                audioSource.pitch = sound.source.pitch;
                audioSource.outputAudioMixerGroup = sound.source.outputAudioMixerGroup;

                //use spatial blend for test how is 2D - 3D the sound
                // audioSource.spatialBlend = 1f; // 1 for only 3d sound - 0 for 2d sound

                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Sound not found or AudioSource not found.");
            }
        }
    }

    void Start()
    {
        // Ottieni l'AudioSource presente sull'oggetto
        audioSource = GetComponent<AudioSource>();
    }
}
