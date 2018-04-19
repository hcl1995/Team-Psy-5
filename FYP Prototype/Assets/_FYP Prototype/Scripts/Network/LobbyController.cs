using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LobbyController : NetworkManager {

	public NetworkDiscovery networkDiscovery;
	protected RectTransform currentPanel;
	static public LobbyController s_Singleton; 
	//public NetworkManager networkManager;
	[Space]
	[Header("UI Reference")]
	public RectTransform LobbyPanel;
	public RectTransform MatchPanel;
	public RectTransform FindMatchPanel;
	public GameObject uiWaiting;
	public GameObject lobbyCanvas;
	public GameObject LoadingCanvas;

	[Space]
	public GameObject player1;
	public GameObject player2;
	public GameObject PlayerObject;
	public int readyPlayer;
	public int intPlayer=0;
	public GameObject player1Character;
	public GameObject player2Character;
	public int player1CharaterProtrait;
	public int player2CharaterProtrait;
	int matchCount = 1;
	public int loadReady;
	public int allReadyEnd;
	public string levelSelectString = "DragonBallLevel";

	public List<GameObject> playerNetwork = new List<GameObject> ();
	public List<GameObject> playerChara = new List<GameObject> ();
	public List<GameObject> playerCharacterSelector = new List<GameObject> ();
	public List<Sprite> selectedCharacterSprite = new List<Sprite> ();

	public CinemachineTargetGroup targetGroup;
	public int isLoadScreenOn;
	// Use this for initialization
	void Start () {
		s_Singleton = this;
		networkDiscovery.Initialize ();
		currentPanel = LobbyPanel;
	}

	public void startHost (){
		base.StartHost();
		LocalPlayerInfo.singleton.playerNum = 1;
	}

	public void CancelFromMatchFinding(){
		changeTo (LobbyPanel);
		networkDiscovery.StopBroadcast ();
		networkDiscovery.Initialize ();
	}

	public void OnBackToMainMenu(){
		base.StopHost();
		base.StopClient ();
		SceneManager.LoadScene(1);
		LocalPlayerInfo.singleton.SelfDestroy ();
		LoadingScreenCanvas.instance.SelfDestroy ();
		LobbyCanvas.instance.SelfDestroy ();
		Destroy (gameObject);
	}

	public override void OnStartHost()
	{
		networkDiscovery.StartAsServer ();
		changeTo (MatchPanel);
	}

	public void startClient(){
		networkDiscovery.StartAsClient ();
		changeTo (FindMatchPanel);
		uiWaiting.SetActive (false);
		LocalPlayerInfo.singleton.playerNum = 2;
	}

	public void changeTo(RectTransform newPanel){
		LevelSelector.instance.OffLevelSelect ();
		player1CharaterProtrait = 0;
		player2CharaterProtrait = 0;
		if (currentPanel != null)
		{
			currentPanel.gameObject.SetActive(false);
		}

		if (newPanel != null)
		{
			newPanel.gameObject.SetActive(true);
		}

		currentPanel = newPanel;
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log ("OnClientConnect");
		ClientScene.Ready (conn);
		ClientScene.AddPlayer (conn, 0);
		if (numPlayers > 1) {
			uiWaiting.SetActive (false);
		}
	}

	public override void OnServerConnect(NetworkConnection conn){
		Debug.Log ("OnServerConnect");
		if (numPlayers > 0) {
			uiWaiting.SetActive (false);
			networkDiscovery.StopBroadcast ();
		}
		intPlayer++;
		Debug.Log (base.numPlayers);


	}

	public override void OnServerDisconnect(NetworkConnection conn){		
		networkDiscovery.Initialize ();
		LobbyController.s_Singleton.LoadingCanvas.SetActive (false);
		if (SceneManager.GetActiveScene().name != "Lobby")
		{
			foreach (GameObject go in playerChara) {
				NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerHealth>().pn.conn, go.GetComponent<PlayerHealth>().pn.playerInfo,0);
			}
			base.ServerChangeScene ("Lobby");
			SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_IMMORTALSELECTION);
			//SoundManager.instance.PlaySpecialBGM(BGMAudioClipID.TOTAL);
		}
		LevelSelector.instance.OffLevelSelect ();
		PlayerInfo.singleton.EnableCharacterSelector ();
		NetworkServer.DestroyPlayersForConnection (conn);
		uiWaiting.SetActive (true);
		networkDiscovery.StartAsServer ();
		playerNetwork.RemoveAt (1);
		playerChara.Clear ();
		foreach (GameObject go in playerNetwork) {
			go.GetComponent<PlayerNetwork> ().textInt = 0;
		}
		PlayerInfo.singleton.OnEnable ();
		intPlayer--;
	}

	public override void OnClientDisconnect(NetworkConnection conn){
		LobbyController.s_Singleton.LoadingCanvas.SetActive (false);
		if (SceneManager.GetActiveScene().name == "Lobby") {
			changeTo (LobbyPanel);
		} else if (SceneManager.GetActiveScene().name != "Lobby") {
			//SceneManager.LoadScene ("Lobby");
			base.ServerChangeScene ("Lobby");
			changeTo (LobbyPanel);
			SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_MAINMENU);
			//SoundManager.instance.PlaySpecialBGM(BGMAudioClipID.TOTAL);
		}
		base.StopClient ();
		networkDiscovery.Initialize ();
	}

	public void OnBackToLobbyMenu(){
		base.ServerChangeScene ("Lobby");
		changeTo (LobbyPanel);
		//SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_MAINMENU);
		//SoundManager.instance.PlaySpecialBGM(BGMAudioClipID.TOTAL);
		base.StopHost();
		base.StopClient ();
		intPlayer = 0;
		playerNetwork.Clear ();
		playerChara.Clear ();
		networkDiscovery.Initialize ();
		SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_MAINMENU);
	}

	public void OnRematch(){
		if (SceneManager.GetActiveScene().name != "Lobby") {
			foreach (GameObject go in playerChara) {
				NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerHealth>().pn.conn, go.GetComponent<PlayerHealth>().pn.playerInfo,0);
			}
			base.ServerChangeScene ("Lobby");
			SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_IMMORTALSELECTION);
			//SoundManager.instance.PlaySpecialBGM(BGMAudioClipID.TOTAL);
			playerChara.Clear();
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		OnServerAddPlayerInternal(conn, playerControllerId);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		OnServerAddPlayerInternal(conn, playerControllerId);
	}

	void OnServerAddPlayerInternal(NetworkConnection conn, short playerControllerId)
	{
		print ("OnServerAddPlayerInternal");
		if (playerControllerId < conn.playerControllers.Count  && conn.playerControllers[playerControllerId].IsValid && conn.playerControllers[playerControllerId].gameObject != null)
		{
			if (LogFilter.logError) { Debug.LogError("There is already a player at that playerControllerId for this connections."); }
			return;
		}

		GameObject playerInfo;
		GameObject playerObject;

		playerObject = (GameObject)Instantiate (PlayerObject, Vector3.zero, Quaternion.identity);
		playerObject.GetComponent<PlayerNetwork> ().Initialize (conn, playerControllerId,intPlayer);
		Transform startPos = GetStartPosition();
		NetworkServer.AddPlayerForConnection (conn, playerObject, playerControllerId);
	}

	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public void PlayerReady(){
		readyPlayer++;
		if (readyPlayer > 1) {
			print ("All Ready");
			PlayerInfo.singleton.RpcLevelSelect ();
			allReadyEnd = 0;
			isLoadScreenOn = 0;
			loadReady = 0;
		}
	}
	public void PlayerUnready(){
		if(readyPlayer>0)
			readyPlayer--;
	}
	public void allLoadScreenOn(){
		isLoadScreenOn++;
		if (isLoadScreenOn == 2) {
			//base.ServerChangeScene ("LevelEditor");
			base.ServerChangeScene (levelSelectString);
			foreach (GameObject go in playerNetwork) {
				NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerNetwork> ().conn, go, 0);
			}
			isLoadScreenOn = 0;
		}
	}

	public void SelectLevel(int level){
		switch (level) {
		case 1:
			levelSelectString = "DragonBallLevel";
			break;
		case 2:
			levelSelectString = "LevelEditor";
			break;
		}
	}

	public void allLoadEnterReady(){
		loadReady++;
		if (loadReady == 2) {
			PlayerNetwork.singleton.RpcDisableLoad ();
			loadReady = 0;
			//StartCoroutine (StartMatchCountDown ());
			//SpawnCharacter ();
		}
	}

	public IEnumerator StartMatchCountDown(){
		yield return new WaitForSeconds(3.0f);
		SpawnCharacter ();
	}

	public void SpawnCharacter(){
		foreach (GameObject go in playerNetwork) {
			GameObject pgo;
			if (go.GetComponent<PlayerNetwork> ().playerNumber == 1) {
				//pgo =  Instantiate (go.GetComponent<PlayerNetwork>().playerCharacter,new Vector3(Random.Range(5,15),0.5f,Random.Range(10,15)), Quaternion.identity);
				pgo = Instantiate (player1Character, new Vector3 (2.5f, 0.5f,12.5f), Quaternion.identity);
			} else {
				pgo = Instantiate (player2Character, new Vector3 (20.0f, 0.5f,12.5f), Quaternion.identity);
			}

			NetworkServer.Spawn (pgo);
			pgo.GetComponent<PlayerHealth> ().playerNumber = go.GetComponent<PlayerNetwork> ().playerNumber;
			pgo.GetComponent<PlayerHealth> ().pn = go.GetComponent<PlayerNetwork> ();
			playerChara.Add (pgo);
			NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerNetwork>().conn, pgo,0);
		}

	}

	public void checkPlayerConditionNew(int playerNumber){
		foreach (GameObject go2 in playerNetwork) {
			if (go2.GetComponent<PlayerNetwork> ().playerNumber != playerNumber) {
				go2.GetComponent<PlayerNetwork> ().WinCount++;
				go2.GetComponent<PlayerNetwork> ().textInt = 1;
			} else {
				go2.GetComponent<PlayerNetwork> ().textInt = 2;
			}
		}			

		foreach(GameObject go in playerChara) {
			NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerHealth>().pn.conn, go.GetComponent<PlayerHealth>().pn.transform.gameObject,0);
		}
		matchCount++;
		//StartCoroutine(serverChangeScene());
		//base.ServerChangeScene ("Lobby");
	}

	public int SetPlayerCharacter(int playerCharacter, int playerNumber){
		int selectedCharacterInt = playerCharacter;

//		if (matchCount % 3 == 0) {
//			if (playerCharacter == 1) {
//				playerCharacter = 3;
//			}else if(playerCharacter == 2){
//				playerCharacter = 4;
//			}
//		}

		if (playerNumber == 1) {
			player1Character = playerCharacterSelector[playerCharacter];
			player1CharaterProtrait = playerCharacter;
			LocalPlayerInfo.singleton.player1 = playerCharacter;

		} else if(playerNumber==2){
			player2Character = playerCharacterSelector[playerCharacter];
			player2CharaterProtrait = playerCharacter;
			LocalPlayerInfo.singleton.player2 = playerCharacter;
		}
		if (player1Character == player2Character) {
			player2Character = playerCharacterSelector [playerCharacter+2];
		}
		//RpcCharacterProtrait (playerCharacter, playerNumber);
		return playerCharacter;
	}


	public void EndMatchAllReady(){
		allReadyEnd++;
		if (allReadyEnd == 2) {
			base.ServerChangeScene ("Lobby");
			foreach(GameObject go in playerChara) {
				NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerHealth>().pn.conn, go.GetComponent<PlayerHealth>().pn.playerInfo,0);
			}
			foreach (GameObject go in playerNetwork) {
				go.GetComponent<PlayerNetwork> ().textInt = 0;
			}
			allReadyEnd = 0;
			playerChara.Clear();
		}
	}

	public IEnumerator serverChangeScene()
	{
		yield return new WaitForSeconds(5f);
		base.ServerChangeScene ("Lobby");
		foreach (GameObject go in playerNetwork) {
			go.GetComponent<PlayerNetwork> ().textInt = 0;
		}
	}
}