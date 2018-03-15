using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundEffect : MonoBehaviour {

	AudioSource sfxAudioSource;

	void Awake () {
		sfxAudioSource = GetComponent<AudioSource> ();
	}

	public void PlaySFX (SFXAudioClipID audioClipID)
	{
		sfxAudioSource.PlayOneShot(SoundManager.instance.sfxAudioClipInfoList[(int)audioClipID].audioClip,
			SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
	}
}