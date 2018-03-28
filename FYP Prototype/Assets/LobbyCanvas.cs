using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {
	private static LobbyCanvas instance;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		if (instance == null) {
			instance = this;
		} else {
			DestroyObject (gameObject);
		}
	}
}
