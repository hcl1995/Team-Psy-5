using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenCanvas : MonoBehaviour {
	public static LoadingScreenCanvas instance;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		if (instance == null) {
			instance = this;
		} else {
			DestroyObject (gameObject);
		}
	}

	public void SelfDestroy(){
		Destroy (gameObject);
	}
}
