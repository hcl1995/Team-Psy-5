using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatchPanel : MonoBehaviour {

	[SerializeField]
	private FindMatchLayout _gameListLayout;
	public FindMatchLayout GameListLayout{
		get {return _gameListLayout;}
	}

	public void OnClickJoinGame(string ipAddress){
		LobbyController.s_Singleton.networkDiscovery.StopBroadcast ();
		LobbyController.s_Singleton.FindMatchPanel.gameObject.SetActive (false);
		LobbyController.s_Singleton.changeTo(LobbyController.s_Singleton.MatchPanel);

		LobbyController.s_Singleton.networkAddress = ipAddress;
		LobbyController.s_Singleton.StartClient();
	}
}
