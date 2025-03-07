using System;
using UnityEngine;
using System.Collections;
using BaseTemplate;

public class AudioManager : Singleton<AudioManager>
{
	public Sound[] sounds;
	private Sound[] pausedSounds;

    protected override bool isDontDestroyOnLoad => false;
	
    protected override void InitializeInstance()
	{
		base.InitializeInstance();

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.pitch = s.pitch;
			s.source.volume = s.volume;
			s.source.loop = s.loop;
			s.source.playOnAwake = s.playOnAwake;
			s.source.outputAudioMixerGroup = s.mixerGroup;
			if (s.clips.Length > 0)
			{
				s.source.clip = s.clips[0];  // First clip default
			}
		}
	}

	void FixedUpdate()
	{
		foreach (Sound s in sounds)
		{
			if (s.source.isPlaying) // Applica le modifiche solo ai suoni in riproduzione
			{
				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
			}
		}
	}

	public void Play(string sound)
    {
        Sound s = GetSoundByName(sound);
        if (s == null || s.clips.Length == 0)
        {
            Debug.LogWarning("Sound: " + sound + " not found or has no clips!");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, s.clips.Length); // Scegli una clip casuale
        s.source.clip = s.clips[randomIndex]; // Assegna la clip all'AudioSource

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

	public void StopPlaying(string sound)
	{
		Sound s = GetSoundByName(sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop();
	}

	public void StopFadeOut(string sound, float FadeTime)
	{
		Sound s = GetSoundByName(sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		StartCoroutine(FadeOut(s, FadeTime));
	}


	IEnumerator FadeOut(Sound fs, float FadeTime)
	{
		float startVolume = fs.source.volume;

		while (fs.source.volume > 0.001)
		{
			fs.source.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}
		fs.source.Stop();
	}

	public void PlayFadeIn(string sound, float FadeTime)
	{
		Sound s = GetSoundByName(sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

		StartCoroutine(FadeIn(s, FadeTime));
	}

	IEnumerator FadeIn(Sound fs, float FadeTime)
	{
		float target = fs.source.volume;
		fs.source.volume = 0f;
		fs.source.Play();
		while (fs.source.volume < target)
		{
			fs.source.volume += Time.deltaTime * FadeTime;

			yield return null;
		}
	}

	public void Pause(string sound)
	{
		Sound s = GetSoundByName(sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Pause();
	}

	public void waitPlaying(string sound)
	{
		Sound s = GetSoundByName(sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (!s.source.isPlaying)
		{
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
			s.source.Play();
		}
	}

	public void pauseAll()
	{
		foreach (Sound s in sounds)
		{
			if (s.name != "OnClickMenu" && s.name != "OnQuitGame" && s.name != "OnBackMenu" && s.source.isPlaying)
			{
				s.source.volume = s.source.volume / 100 * 30;
			}
		}
	}

	public void stopPauseAll()
	{
		foreach (Sound s in sounds)
		{
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		}
	}

	public Sound GetSoundByName(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);

		if (s != null && s.clips.Length > 0)
		{
			int randomIndex = UnityEngine.Random.Range(0, s.clips.Length);
			s.source.clip = s.clips[randomIndex]; 
		
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        	s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		}

		return s;
	}
}