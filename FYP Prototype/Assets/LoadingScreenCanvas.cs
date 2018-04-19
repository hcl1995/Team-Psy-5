using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenCanvas : MonoBehaviour {
	private static LoadingScreenCanvas _instance = null;
	public static LoadingScreenCanvas instance{
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");

			return _instance;
		}
	}
	public Canvas canvas;
	// Use this for initialization
	void Awake () {
		
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if(instance!=this){
			Destroy(this.gameObject);
		}
	}

	public void SelfDestroy(){
		Destroy (this.gameObject);
	}
}
