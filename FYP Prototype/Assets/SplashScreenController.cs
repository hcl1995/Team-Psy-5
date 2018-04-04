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
	void Awake(){
		DontDestroyOnLoad (transform.parent.gameObject);
	}
	void Start () {
		SceneManager.LoadScene(1);
	}
	
	// Update is called once per frame
	void Update () {
		TimeElepsed = TimeElepsed + Time.deltaTime;
		if (TimeElepsed >= 3) {
			gameObject.GetComponent<Image> ().sprite = GameSplash;
		}
		if (TimeElepsed >= 6 && !isStart) {
			isStart = true;
			gameObject.SetActive (false);
		}
		if (Input.anyKeyDown) {
			TimeElepsed += 3.0f;
		}
	}
}
