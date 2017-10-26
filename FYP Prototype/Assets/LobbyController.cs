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
	[Space]
	public GameObject player1;
	public GameObject player2;
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
		ClientScene.Ready (conn);
		ClientScene.AddPlayer (conn, (short)numPlayers);
		if (numPlayers > 1) {
			uiWaiting.SetActive (false);
		}
		Debug.Log (numPlayers);
	}

	public void OnPlayerDisconnect(NetworkPlayer id){
		uiWaiting.SetActive (true);
		Network.DestroyPlayerObjects (id);
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		GameObject player;
		Debug.Log ("playerControllerId = " + playerControllerId);
		if (uiPlayer1.GetComponentInChildren<Button> () == null) {
			player = (GameObject)Instantiate (player1, uiPlayer1.transform, false);
		} else {
			player = (GameObject)Instantiate (player2, uiPlayer2.transform, false);
		}
		NetworkServer.Spawn (player);
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
	}
}
