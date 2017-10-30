using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

	[Space]
	public GameObject player1;
	public GameObject player2;
	public int readyPlayer;
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

		GameObject player;
		Transform startPos = GetStartPosition();
		if (uiPlayer1.GetComponentInChildren<NetworkIdentity> () == null) {
			player = (GameObject)Instantiate (player1,Vector3.zero, Quaternion.identity);
		} else {
			player = (GameObject)Instantiate (player2,Vector3.zero, Quaternion.identity);
		}
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public void PlayerReady(){
		readyPlayer++;
		if (readyPlayer > 1) {
			print ("All Ready");
			base.ServerChangeScene ("Main02");
		}
	}

	public void PlayerUnready(){
		readyPlayer--;
	}
}
