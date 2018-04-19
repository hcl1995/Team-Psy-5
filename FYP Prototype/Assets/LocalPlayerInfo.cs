using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerInfo : MonoBehaviour {
	private static LocalPlayerInfo _singleton = null;
	static public LocalPlayerInfo singleton{
		get {
			if (_singleton == null)
				Debug.LogError ("A script is trying to access the SoundManager which isn't present in this scene!");

			return _singleton;
		}
	}
	public int playerNum = 0;
	public int player1;
	public int player2;
	public hitIndicator hit;
	// Use this for initialization
	void Awake () {		
		if (_singleton == null) {
			_singleton = this;
			DontDestroyOnLoad (gameObject);
		} else if(singleton!=this){
			Destroy(this.gameObject);
		}

		//DontDestroyOnLoad(transform.gameObject);
	}

	public void assginPlayerNumber(int playerNumber){
		playerNum = playerNumber;
	}

	//[ClientRpc]
	public void enableLoading(){
		LobbyController.s_Singleton.LoadingCanvas.SetActive (true);
		//Time.timeScale = 0;
	}

	public void disableLoading(){
		LobbyController.s_Singleton.LoadingCanvas.SetActive (false);
		StartTimer.singleton.StartMatchTimer ();
	}

	public void OnHit(){
		hit.OnHit ();
	}

	public void SelfDestroy(){
		Destroy (this.gameObject);
	}
}
