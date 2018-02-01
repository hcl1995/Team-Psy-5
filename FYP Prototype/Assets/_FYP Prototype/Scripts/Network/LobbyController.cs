using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cinemachine;

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
	public Transform uiPlayer1;
	public Transform uiPlayer2;
	public GameObject uiWaiting;
	public GameObject lobbyCanvas;
	public GameObject gameCanvas;
	public RectTransform playerHealth1;
	public RectTransform playerHealth2;
	public Transform playerWin01;
	public Transform playerWin02;

	[Space]
	public GameObject player1;
	public GameObject player2;
	public GameObject PlayerObject;
	public int readyPlayer;
	public int intPlayer=0;
	public GameObject player1Character;
	public GameObject player2Character;

	public List<GameObject> playerNetwork = new List<GameObject> ();
	public List<GameObject> playerChara = new List<GameObject> ();

	public CinemachineTargetGroup targetGroup;

	// Use this for initialization
	void Start () {
		s_Singleton = this;
		networkDiscovery.Initialize ();
		currentPanel = LobbyPanel;
	}

	public void startHost (){
		base.StartHost();
	}

	public override void OnStartHost()
	{
		networkDiscovery.StartAsServer ();
		changeTo (MatchPanel);
		Debug.Log("OnPlayerConnected");
	}

	public void startClient(){
		networkDiscovery.StartAsClient ();
		changeTo (FindMatchPanel);
		uiWaiting.SetActive (false);
	}

	public void changeTo(RectTransform newPanel){
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
		Debug.Log (base.numPlayers);
	}

	public override void OnServerConnect(NetworkConnection conn){
		Debug.Log ("OnServerConnect");
		if (numPlayers > 0) {
			uiWaiting.SetActive (false);
		}
		intPlayer++;
		Debug.Log (base.numPlayers);
	}

	public void OnPlayerDisconnect(NetworkPlayer id){
		uiWaiting.SetActive (true);
		Network.DestroyPlayerObjects (id);
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

	public void PlayerReady(){
		readyPlayer++;
		if (readyPlayer > 1) {
			print ("All Ready");
			base.ServerChangeScene ("LevelEditor");
			foreach (GameObject go in playerNetwork) {
				NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerNetwork> ().conn, go, 0);
			}
			//gameCanvas.SetActive (true);
			readyPlayer = 0;
		}
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

	public void PlayerUnready(){
		readyPlayer--;
	}

	public void checkPlayerCondition(){
		foreach (GameObject go in playerChara) {
			if (go.GetComponent<PlayerHealth> ().currentHealth > 0) {
				foreach (GameObject go2 in playerNetwork) {
					if (go2.GetComponent<PlayerNetwork> ().playerNumber == go.GetComponent<PlayerHealth> ().playerNumber) {
						go2.GetComponent<PlayerNetwork> ().WinCount++;
					}
				}
			} else {

			}
		}
		foreach (GameObject go in playerChara) {
			NetworkServer.ReplacePlayerForConnection (go.GetComponent<PlayerHealth>().pn.conn, go.GetComponent<PlayerHealth>().pn.playerInfo,0);
		}
		playerChara = new List<GameObject> ();
		StartCoroutine(serverChangeScene());
		//base.ServerChangeScene ("Lobby");
	}

	public IEnumerator serverChangeScene()
	{
		yield return new WaitForSeconds(5f);
		base.ServerChangeScene ("Lobby");
	}
}