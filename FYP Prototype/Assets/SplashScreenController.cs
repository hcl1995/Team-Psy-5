using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour {
	public Sprite GameSplash;
	public float TimeElepsed = 0;
	bool isStart = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TimeElepsed = TimeElepsed + Time.deltaTime;
		if (TimeElepsed >= 3) {
			gameObject.GetComponent<Image> ().sprite = GameSplash;
		}
		if (TimeElepsed >= 6 && !isStart) {
			isStart = true;
			SceneManager.LoadScene(1);
		}
		if (Input.anyKeyDown) {
			TimeElepsed += 3.0f;
		}
	}
}
