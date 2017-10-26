using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListPanel : MonoBehaviour {
	public Prototype.NetworkLobby.LobbyManager lobbyManager;
	public RectTransform lobbyPanel;
	[SerializeField]
	private GameListLayout _gameListLayout;
	public GameListLayout GameListLayout{
		get {return _gameListLayout;}
	}

	public void OnClickJoinGame(string ipAddress){
		Prototype.NetworkLobby.LobbyManager.s_Singleton.gameListPanel.gameObject.SetActive (false);
		lobbyManager.ChangeTo(lobbyPanel);

		lobbyManager.networkAddress = ipAddress;
		lobbyManager.StartClient();

		lobbyManager.backDelegate = lobbyManager.StopClientClbk;
		lobbyManager.DisplayIsConnecting();

		lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
	}
}
