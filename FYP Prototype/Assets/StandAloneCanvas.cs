using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAloneCanvas : MonoBehaviour {
	private static StandAloneCanvas _instance = null;
	public static StandAloneCanvas instance{
		get {
			if (_instance == null)
				Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");

			return _instance;
		}
	}
	public GameObject Leave;
	public GameObject GameManager;
	// Use this for initialization
	void Awake () {
		
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if(instance!=this){
			Destroy (this.gameObject);
		}
	}

	void Update(){
		if (Input.GetKeyUp (KeyCode.Escape)) {			
			if (Leave.activeSelf) {
				Leave.SetActive (false);
			} else if(!Leave.activeSelf){
				Leave.SetActive (true);
				LoadingScreenCanvas.instance.canvas.sortingOrder = 100;
			}
		}
	}

	public void OnLeaveButton(){
		LobbyController.s_Singleton.OnBackToLobbyMenu ();
		Leave.SetActive (false);
		LobbyController.s_Singleton.LoadingCanvas.SetActive (false);
	}

	public void OnRematchButton(){
		PlayerControl.singleton.CmdRematch ();
		Leave.SetActive (false);
		LobbyController.s_Singleton.LoadingCanvas.SetActive (false);
	}

	public void OnReturnButton()
	{
		Leave.SetActive (false);
	}
}
