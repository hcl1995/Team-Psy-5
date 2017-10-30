﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfo : NetworkBehaviour {
	public List<Transform> infoLoction = new List<Transform>();
	[SyncVar(hook="Ready")]
	public bool ready = false;
	public Image buttonImage;
	// Use this for initialization
	void OnEnable(){
		SceneManager.activeSceneChanged += OnSceneChange;
	}
	void Awake(){
		
	}
	void Start () {
		infoLoction.Add(LobbyController.s_Singleton.uiPlayer1);
		infoLoction.Add(LobbyController.s_Singleton.uiPlayer2);
		if (LobbyController.s_Singleton.uiPlayer1.GetComponentInChildren<NetworkIdentity> () == null) {
			transform.SetParent (infoLoction [0]);
			transform.localPosition = Vector3.zero;
		} else {
			transform.SetParent (infoLoction [1]);
			transform.localPosition = Vector3.zero;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickPlayerReady(){
		if (!isLocalPlayer)
			return;

		if (!ready) {
			CmdReadyState (true);
			CmdPlayerReady ();
			//RpcReady ();
			buttonImage.color = Color.green;

		} else if (ready) {
			CmdReadyState (false);
			CmdPlayerUnready ();
			//RpcReady ();
			buttonImage.color = Color.red;
		}
	}

	[Command]
	void CmdPlayerReady(){
		LobbyController.s_Singleton.PlayerReady ();

	}

	[Command]
	void CmdPlayerUnready(){
		LobbyController.s_Singleton.PlayerUnready ();
	}

	[Command]
	void CmdReadyState(bool r){		
		ready = r;
	}
		
	public void Ready(bool r){
		ready = r;
		if (ready) {
			buttonImage.color = Color.green;
		} else if (!ready) {
			buttonImage.color = Color.red;
		}
	}

	public void OnSceneChange(Scene scene1, Scene scene2){
		Debug.Log (scene1.name);
		Debug.Log (scene2.name);
		if (scene2.name == "Main02") {
			LobbyController.s_Singleton.lobbyCanvas.SetActive (false);
		}
	}
}
