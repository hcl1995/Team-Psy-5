using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject keyMapping;

	public void GameStart()
	{
		SceneManager.LoadScene(1);
	}

	public void Setting()
	{
		keyMapping.SetActive(true);
	}

	public void SettingBack()
	{
		keyMapping.SetActive(false);
	}

	public void ResetKey()
	{
		
	}

	public void Exit()
	{
		Application.Quit();	
	}
}