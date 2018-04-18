using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerInfo : MonoBehaviour {

	static public LocalPlayerInfo singleton; 
	public int playerNum = 0;
	public int player1;
	public int player2;
	public hitIndicator hit;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		if (singleton == null) {
			singleton = this;
		} else {
			Destroy(gameObject);
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
		Destroy (gameObject);
	}
}
