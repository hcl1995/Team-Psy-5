﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfo : NetworkBehaviour {
	static public PlayerInfo singleton; 
	public List<Transform> infoLoction = new List<Transform>();
	[SyncVar(hook="Ready")]
	public bool ready = false;
	public Image buttonImage;
	public GameObject readyButton;
	public GameObject playerNetwork;
	public GameObject localPlayerIndicator;
	public GameObject readyIndicator;
	public GameObject canvas;
	[SyncVar]
	public int playerCharacter;
	public GameObject CharaterSelector;
	[SyncVar(hook="SelectedCharacter")]
	public int selectedCharacterInt;
	public Image characterSprite;
	public RectTransform rt;
	[SyncVar]
	public int playerNumber = 0;
	// Use this for initialization
	void OnEnable(){
		canvas.SetActive (true);
		ready = false;
		buttonImage.color = Color.red;
		SoundManager.instance.PlayBGM (BGMAudioClipID.BGM_IMMORTALSELECTION);
	}
	void Awake(){
		rt = (RectTransform)transform;
	}
	void Start () {
		rt.SetParent (LobbyController.s_Singleton.MatchPanel.transform,false);

		if (isLocalPlayer) {
			SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_IMMORTALSELECTION);
			singleton = this;
			localPlayerIndicator.SetActive (true);
			CharaterSelector.SetActive (true);
			readyButton.SetActive (true);
			CmdSetPlayerNumber (LocalPlayerInfo.singleton.playerNum);
		}
		if (!isLocalPlayer) {
			readyIndicator.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		transform.position = Vector3.zero;
//
		transform.localPosition = Vector3.zero;

		if (isLocalPlayer)
			return;
		
		if (ready) {
			buttonImage.color = Color.green;
			readyIndicator.GetComponent<Image> ().color = Color.green;
		} else if (!ready) {
			buttonImage.color = Color.red;
			readyIndicator.GetComponent<Image> ().color = Color.red;
		}
		characterSprite.sprite = LobbyController.s_Singleton.selectedCharacterSprite [selectedCharacterInt];
	}

	public void OnClickPlayerReady(){
		if (!isLocalPlayer)
			return;
		if (selectedCharacterInt == 0)
			return;

		if (!ready) {	
			CmdReadyState (true);

			buttonImage.color = Color.green;

		} else if (ready) {	
			CmdReadyState (false);

			buttonImage.color = Color.red;
		}
	}

	public void setSelectCharacter(int character){
		playerCharacter = character;
		if(isLocalPlayer)
			CmdSetSelectCharacter (playerCharacter, playerNumber);
	}

	[Command]
	void CmdReadyState(bool r){		
		ready = r;
		if (ready) {
			LobbyController.s_Singleton.PlayerReady ();
		} else if (!ready) {
			LobbyController.s_Singleton.PlayerUnready ();
		}
	}

	[Command]
	void CmdSetSelectCharacter(int character,int playerNumber){
		selectedCharacterInt = LobbyController.s_Singleton.SetPlayerCharacter (character, playerNumber);
		//selectedCharacterInt = character;
		playerCharacter = character;
	}
	[Command]
	void CmdSetPlayerNumber(int i){
		playerNumber = i;
	}
	public void Ready(bool r){
		ready = r;
		if (ready) {
			buttonImage.color = Color.green;
			readyIndicator.GetComponent<Image> ().color = Color.green;
		} else if (!ready) {
			buttonImage.color = Color.red;
			readyIndicator.GetComponent<Image> ().color = Color.red;
		}
	}

	public void SelectedCharacter(int i){
		selectedCharacterInt = i;
		characterSprite.sprite = LobbyController.s_Singleton.selectedCharacterSprite [i];
	}

	public void OnSceneChange(Scene scene1, Scene scene2){

	}
	[Command]
	public void CmdSelectLevel(int level){
		LobbyController.s_Singleton.SelectLevel (level);
		RpcSelectLevel (level);
	}

	[ClientRpc]
	public void RpcSelectLevel(int level){
		LevelSelector.instance.ChangeSelectedLevelImage (level);
	}

	[ClientRpc]
	public void RpcEnableLoading(){
		LevelSelector.instance.OffLevelSelect ();
		LocalPlayerInfo.singleton.enableLoading ();
	}

	[Command]
	public void CmdLoadScreenOn(){
		LobbyController.s_Singleton.allLoadScreenOn ();
	}
	[ClientRpc]
	public void RpcPlayThisBGM(BGMAudioClipID bgm){
		SoundManager.instance.PlayBGM (bgm);
	}

	[Command]
	public void CmdPlayhisBGM(BGMAudioClipID bgm){
		RpcPlayThisBGM(bgm);
	}

	public void PlayhisBGM(BGMAudioClipID bgm){
		if(isServer)
			CmdPlayhisBGM(bgm);
	}

	[ClientRpc]
	public void RpcLevelSelect(){
		LevelSelector.instance.OnReadyLevelSelect ();
	}

	public void DisableCharacterSelector(){
		canvas.SetActive (false);
	}

	public void EnableCharacterSelector(){
		canvas.SetActive (true);
		ready = false;
		buttonImage.color = Color.red;
	}
}
