using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour {
	static public PlayerInfo singleton; 
	public List<Transform> infoLoction = new List<Transform>();
	[SyncVar]
	public bool ready = false;
	public Image buttonImage;
	public GameObject readyButton;
	public GameObject quitButton;
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
	public int player1Select;
	[SyncVar]
	public int player2Select;

	[SyncVar]
	public int playerNumber = 0;
	Color playerColor;
	Color oppColor;
	// Use this for initialization
	public void OnEnable(){
		if (isLocalPlayer) {
			canvas.SetActive (true);
			CmdReadyState (false);
			localPlayerIndicator.GetComponent<Image> ().color = Color.black;
			SoundManager.instance.PlayBGM (BGMAudioClipID.BGM_IMMORTALSELECTION);
		}
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
			quitButton.SetActive(true);
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
		if (isServer) {
			CmdGetSelect ();
		}

		int playerNumberForSprite = 0;
		if (playerNumber == 2&&selectedCharacterInt>0)
			playerNumberForSprite = 2;
		if (playerNumber == 2 && selectedCharacterInt == 0) {
			playerNumberForSprite = 5;
		}
		transform.localPosition = Vector3.zero;
		if (playerNumber == 1) {
			playerColor = Color.blue;
			oppColor = Color.red;
		}else if (playerNumber == 2) {
			oppColor = Color.blue;
			playerColor = Color.red;
		}
		if (isLocalPlayer)
			return;
		
		if (ready) {
			readyIndicator.GetComponent<Image> ().color = playerColor;
		} else if (!ready) {
			readyIndicator.GetComponent<Image> ().color = Color.black;
		}

		characterSprite.sprite = LobbyController.s_Singleton.selectedCharacterSprite [selectedCharacterInt+playerNumberForSprite];
	}

	public void OnClickPlayerReady(){
		if (!isLocalPlayer)
			return;
		if (selectedCharacterInt == 0)
			return;

		if (!ready) {	
			CmdReadyState (true);
			localPlayerIndicator.GetComponent<Image> ().color = playerColor;

		} else if (ready) {	
			CmdReadyState (false);
			localPlayerIndicator.GetComponent<Image> ().color = Color.black;
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
			readyIndicator.GetComponent<Image> ().color = oppColor;
		} else if (!ready) {

			readyIndicator.GetComponent<Image> ().color = Color.black;
		}
	}

	public void SelectedCharacter(int i){
		int playerNumberForSprite = 0;
		if (playerNumber == 2 && i > 0) {
			playerNumberForSprite = 2;
		} 
		if (playerNumber == 2 && i == 0) {
			playerNumberForSprite = 5;
		}
		selectedCharacterInt = i;
		characterSprite.sprite = LobbyController.s_Singleton.selectedCharacterSprite [i+playerNumberForSprite];
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
	}

	[Command]
	public void CmdGetSelect(){
		player1Select = LobbyController.s_Singleton.player1CharaterProtrait;
		player2Select = LobbyController.s_Singleton.player2CharaterProtrait;
	}

	public void BringMeBack()
	{
		LobbyController.s_Singleton.OnBackToLobbyMenu();
	}
}
