using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public float defaultVolume = 0.8f;


	[Header ("UI Contents")]
	public GameObject mainMenu;
	public GameObject settings;
	public GameObject audioSetting;
	public GameObject controlSetting;
	public GameObject credit;


	[Header ("Volume Sliders")]
	public Slider masterVolumeSlider;
	public Slider bgmVolumeSlider;
	public Slider sfxVolumeSlider;


	public void GameStart()
	{
		SceneManager.LoadScene(2);
	}

	public void Training()
	{
		
	}

	public void Setting()
	{
		mainMenu.SetActive(false);
		settings.SetActive(true);
	}

	public void AudioSetting()
	{
		settings.SetActive(false);
		audioSetting.SetActive(true);

		masterVolumeSlider.value = SoundManager.instance.GetMasterVolume();
		bgmVolumeSlider.value = SoundManager.instance.GetMusicVolume();
		sfxVolumeSlider.value = SoundManager.instance.GetSoundVolume();
	}

	public void AudioSettingBack()
	{
		audioSetting.SetActive(false);
		settings.SetActive(true);
	}

	public void AudioSettingReset()
	{
		masterVolumeSlider.value = 1;
		sfxVolumeSlider.value = defaultVolume;
		bgmVolumeSlider.value = defaultVolume;
	}

	public void OnMasterVolumeChanged () {
		SoundManager.instance.SetMasterVolume (masterVolumeSlider.value);
	}

	public void OnSoundVolumeChanged () {
		SoundManager.instance.SetSoundVolume (sfxVolumeSlider.value);
	}

	public void OnMusicVolumeChanged () {
		SoundManager.instance.SetMusicVolume (bgmVolumeSlider.value);
	}

	public void ControlSetting()
	{
		controlSetting.SetActive(true);
	}

	public void ControlSettingBack()
	{
		controlSetting.SetActive(false);
	}

	public void ControlSettingReset()
	{
		
	}

	public void SettingBack()
	{
		settings.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void Exit()
	{
		Application.Quit();	
	}

	public void Credit(){
		credit.SetActive (true);
	}

	void Update(){
		if (credit.activeSelf) {
			if (Input.anyKeyDown) {
				credit.SetActive (false);
			}
		}
	}
}