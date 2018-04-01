using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundEffect : MonoBehaviour {

	AudioSource sfxAudioSource;
	AudioSource environmentSfxAudioSource;

	public List<AudioClip> selfServiceClip = new List<AudioClip>();
	public AudioSource[] audioSourceList;

	void Awake () {
		audioSourceList = GetComponents<AudioSource>();
		sfxAudioSource = audioSourceList[0];

		if(audioSourceList.Length > 1)
		{
			environmentSfxAudioSource = audioSourceList[1];
		}
	}

	// enum wasn't good for sound manager..
	public void PlaySFX (SFXAudioClipID audioClipID)
	{
		sfxAudioSource.PlayOneShot(SoundManager.instance.sfxAudioClipInfoList[(int)audioClipID].audioClip,
			SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
	}

	public void PlaySFXClip (AudioClip audioClipID)
	{
		sfxAudioSource.PlayOneShot(audioClipID, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
	}

	public void PlayEnvironmentSFXClip (AudioClip audioClipID)
	{
		environmentSfxAudioSource.PlayOneShot(audioClipID, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
	}

	public void StopSFXClip ()
	{
		environmentSfxAudioSource.Stop();
	}
}