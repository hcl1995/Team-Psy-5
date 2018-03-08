using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerNetwork : NetworkBehaviour {

	public NetworkConnection conn;
	public short playerControllerId;
	public GameObject player1;
	public GameObject player2;
	public GameObject thisGO;
	public GameObject playerCharacter;
	public GameObject playerInfo;
	public Text winText;
	public List<Transform> winLocation = new List<Transform>();

	[SyncVar(hook = "GetPlayerWin")]
	public int WinCount = 0;
	[SyncVar(hook = "GetPlayerNumber")]
	public int playerNumber = 0;

	public int startOnce = 0;

	void Awake(){	
		thisGO = gameObject;
		DontDestroyOnLoad (gameObject);
		winLocation.Add (LobbyController.s_Singleton.playerWin01);
		winLocation.Add (LobbyController.s_Singleton.playerWin02);
	}

	void Start () {
		if (playerNumber == 1) {
			transform.SetParent (winLocation [0]);
		} else {
			transform.SetParent (winLocation [1]);
		}
		transform.localPosition = Vector3.zero;
		if (!isLocalPlayer)
			return;
		
		CmdspawnPlayerInfo ();
		SceneManager.activeSceneChanged += OnSceneChange;
		startOnce++;
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
		if (scene2.name == "LevelEditor") {
			Debug.Log ("Is Me");
			LobbyController.s_Singleton.lobbyCanvas.SetActive (false);
			CmdSpawnCharacter ();
		}
		if (scene2.name == "Lobby") {
			LobbyController.s_Singleton.lobbyCanvas.SetActive (true);
		}
	}

	void Update(){
		transform.localPosition = Vector3.zero;
		winText.text = "Win : " + WinCount;
	}

	[Command]
	void CmdspawnPlayerInfo(){
		GameObject go;
		if (playerNumber == 1) {
			playerInfo = Instantiate (player1, Vector3.zero,Quaternion.identity);
			playerInfo.GetComponent<PlayerInfo> ().playerNetwork = thisGO;
			NetworkServer.ReplacePlayerForConnection (conn, playerInfo,0);
		} else {
			playerInfo = Instantiate (player2, Vector3.zero,Quaternion.identity);
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
		winText.text = "Win : " + i;
	}
}
