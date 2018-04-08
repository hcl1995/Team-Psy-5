using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAloneCanvas : MonoBehaviour {
	private static StandAloneCanvas instance;
	public GameObject Leave;
	public GameObject GameManager;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		if (instance == null) {
			instance = this;
		} else {
			DestroyObject (gameObject);
		}
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Escape)) {			
			if (Leave.activeSelf) {
				Leave.SetActive (false);
			} else if(!Leave.activeSelf){
				Leave.SetActive (true);
			}
		}
	}

	public void OnLeaveButton(){
		LobbyController.s_Singleton.OnBackToLobbyMenu ();
		Leave.SetActive (false);
	}

	public void OnRematchButton(){
		PlayerControl.singleton.CmdRematch ();
		Leave.SetActive (false);
	}
}
