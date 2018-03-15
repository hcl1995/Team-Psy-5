using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMAudioClipID
{
	BGM_MAINMENU = 0,
	BGM_IMMORTALSELECTION,
	BGM_INGAME,
	TOTAL
}

public enum SFXAudioClipID
{
	SFX_ATTACK = 0,
	SFX_ATTACKMISSED,
	SFX_DASH,
	SFX_SKILL01,
	SFX_SKILL02,
	SFX_ULTIMATE,
	SFX_DEATH,
	TOTAL
}

[System.Serializable]
public class BGMAudioClipInfo
{
	public BGMAudioClipID audioClipID;
	public AudioClip audioClip;
}

[System.Serializable]
public class SFXAudioClipInfo
{
	public SFXAudioClipID audioClipID;
	public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour {
	private static SoundManager _instance = null;
	public static SoundManager instance {
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");

			return _instance;
		}
	}

	[Header ("Volume Setting Keys")]
	public string masterVolumeSetting = "masterVolume";
	public string soundVolumeSetting = "soundVolume";
	public string musicVolumeSetting = "musicVolume";


	[Header ("Audio Clip List")]
	public AudioClip onHitClip; // Not Sure How To "SoundManager" It Without This.
	public AudioSource bgmAudioSource;
	public List<BGMAudioClipInfo> bgmAudioClipInfoList = new List<BGMAudioClipInfo>();
	public List<SFXAudioClipInfo> sfxAudioClipInfoList = new List<SFXAudioClipInfo>();


	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}

		//Application.runInBackground = true;

		PlayBGM(BGMAudioClipID.BGM_MAINMENU);
		bgmAudioSource.ignoreListenerPause = true;
		bgmAudioSource.volume = GetMusicVolume () * GetMasterVolume ();
	}

	public void PlayBGM (BGMAudioClipID audioClipID)
	{
		bgmAudioSource.clip = bgmAudioClipInfoList[(int)audioClipID].audioClip;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	public float GetMasterVolume () {
		return PlayerPrefs.GetFloat (masterVolumeSetting, 1.0f);
	}

	public float GetSoundVolume () {
		return PlayerPrefs.GetFloat (soundVolumeSetting, 1.0f);
	}

	public float GetMusicVolume () {
		return PlayerPrefs.GetFloat (musicVolumeSetting, 0.3f); // 0.7f too noisy
	}

	public void SetMasterVolume (float value) {
		PlayerPrefs.SetFloat (masterVolumeSetting, value);
		bgmAudioSource.volume = GetMusicVolume () * GetMasterVolume ();
	}

	public void SetSoundVolume (float value) {
		PlayerPrefs.SetFloat (soundVolumeSetting, value);
	}

	public void SetMusicVolume (float value) {
		PlayerPrefs.SetFloat (musicVolumeSetting, value);
		bgmAudioSource.volume = GetMusicVolume () * GetMasterVolume ();
	}
}