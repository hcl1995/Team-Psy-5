using System.Collections;
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
	public GameObject readyButton;
	public GameObject playerNetwork;
	public GameObject localPlayerIndicator;
	public GameObject readyIndicator;
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
		ready = false;
		buttonImage.color = Color.red;
	}
	void Awake(){
		rt = (RectTransform)transform;
	}
	void Start () {
		//		infoLoction.Add(LobbyController.s_Singleton.uiPlayer1);
		//		infoLoction.Add(LobbyController.s_Singleton.uiPlayer2);
		//		if (LobbyController.s_Singleton.uiPlayer1.GetComponentInChildren<NetworkIdentity> () == null) {
		rt.SetParent (LobbyController.s_Singleton.MatchPanel.transform,false);
		//			transform.localPosition = Vector3.zero;
		//		} else {
		//			transform.SetParent (infoLoction [1]);
		//			transform.localPosition = Vector3.zero;
		//		}
		if (isLocalPlayer) {
			SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_IMMORTALSELECTION);

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

	public void setSelectCharacter(int character){
		playerCharacter = character;
		if(isLocalPlayer)
			CmdSetSelectCharacter (playerCharacter, playerNumber);
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

	[Command]
	void CmdSetSelectCharacter(int character,int playerNumber){
		selectedCharacterInt = LobbyController.s_Singleton.SetPlayerCharacter (character, playerNumber);
		//selectedCharacterInt = character;
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
		if (!isLocalPlayer)
			return;
		Debug.Log (scene1.name);
		Debug.Log (scene2.name);
		if (scene2.name == "Main02") {
			//NetworkServer.ReplacePlayerForConnection (playerNetwork.GetComponent<PlayerNetwork> ().conn, playerNetwork, 0);
		}
	}
}
