using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {
	private static LobbyCanvas _instance = null;
	public static LobbyCanvas instance{
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");

			return _instance;
		}
	}
	// Use this for initialization
	void Awake () {		
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if(instance!=this){
			Destroy(gameObject);
		}
	}

	public void SelfDestroy(){
		Destroy (gameObject);
	}
}
