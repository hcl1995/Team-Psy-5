using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerNetwork : NetworkBehaviour {

	static public PlayerNetwork singleton; 
	public NetworkConnection conn;
	public short playerControllerId;
	public GameObject player1;
	public GameObject player2;
	public GameObject thisGO;
	public GameObject playerCharacter;
	public GameObject playerInfo;
	public GameObject playerUI;

	public Text endGameText;
	public GameObject Canvas;

	[SyncVar(hook = "GetEndGame")]
	public int textInt = 0;
	[SyncVar(hook = "GetPlayerWin")]
	public int WinCount = 0;
	[SyncVar(hook = "GetPlayerNumber")]
	public int playerNumber = 0;

	public int startOnce = 0;

	void Awake(){	
		thisGO = gameObject;
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		if (isLocalPlayer) {
			singleton = this;
			Canvas.SetActive (true);
			CmdspawnPlayerInfo ();
			SceneManager.activeSceneChanged += OnSceneChange;
			startOnce++;
		}
		transform.localPosition = Vector3.zero;
		


	}
	public void Initialize(NetworkConnection conn, short playerControllerId, int playerNumber){
		this.conn = conn;
		this.playerControllerId = playerControllerId;
		this.playerNumber = playerNumber;
		if (isLocalPlayer) {
			Debug.Log (true);
		}
	}

	void OnSceneChange(Scene scene1, Scene scene2){
		if (scene2.name == "DragonBallLevel" || scene2.name == "LevelEditor")
		{
			Debug.Log ("Is Me");
			LobbyController.s_Singleton.lobbyCanvas.SetActive (false);

			//CmdSpawnCharacter ();
		
		}
		if (scene2.name == "Lobby") {
			LobbyController.s_Singleton.lobbyCanvas.SetActive (true);
		}
	}

	void Update(){
		//transform.localPosition = Vector3.zero;
		switch (textInt) {
		case 0:
			endGameText.text = "";
			break;
		case 1:
			endGameText.text = "You Win";
			break;
		case 2:
			endGameText.text = "You Lose";
			break;
		}
	}

	[Command]
	void CmdspawnPlayerInfo(){
		GameObject go;
		if (playerNumber == 1) {
			playerInfo = Instantiate (player1, LobbyController.s_Singleton.MatchPanel);
			playerInfo.GetComponent<PlayerInfo> ().playerNetwork = thisGO;
			NetworkServer.ReplacePlayerForConnection (conn, playerInfo,0);
		} else {
			playerInfo = Instantiate (player2, LobbyController.s_Singleton.MatchPanel);
			playerInfo.GetComponent<PlayerInfo> ().playerNetwork = thisGO;
			NetworkServer.ReplacePlayerForConnection (conn, playerInfo,0);
		}
		LobbyController.s_Singleton.playerNetwork.Add (thisGO);
		RpcAssign (playerInfo);
	}

	[ClientRpc]
	void RpcAssign(GameObject go){
		go.GetComponent<PlayerInfo> ().playerNetwork = thisGO;
	}

	[Command]
	void CmdSpawnCharacter(){
		LobbyController.s_Singleton.SpawnCharacter ();
	}

	void GetPlayerNumber(int i){
		playerNumber = i;
	}

	void GetPlayerWin(int i){
		print ("GetPlayerWin");
		WinCount = i;
	}

	void GetEndGame(int i){
		textInt = i;

	}

	[Command]
	public void CmdloadingOnEnterReady(){
		LobbyController.s_Singleton.allLoadEnterReady ();
	}

	[ClientRpc]
	public void RpcDisableLoad(){
		LocalPlayerInfo.singleton.disableLoading ();
	}

	[Command]
	public void CmdStartMatchSpawnNow(){
		if (isServer) {
			LobbyController.s_Singleton.SpawnCharacter ();
		}
	}

	[Command]
	public void CmdOnEndMatchReady(){
		LobbyController.s_Singleton.EndMatchAllReady ();
	}
}
