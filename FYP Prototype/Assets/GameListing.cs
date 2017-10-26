using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameListing : MonoBehaviour {

	[SerializeField]
	private Text _gameIP;
	public Text GameIP{
		get{ return _gameIP; }
	}

	public string strGameIP{ get; private set;}
	public bool Updated{ get; set; }

	private void Start(){
		GameObject gameListPanelObj = Prototype.NetworkLobby.LobbyManager.s_Singleton.gameListPanel.gameObject;//transform.parent.GetComponent<GameListPanel> ().gameObject;
		if (gameListPanelObj == null)
			return;

		GameListPanel gameListPanel = gameListPanelObj.GetComponent<GameListPanel> ();
		Button button = GetComponent<Button>();
		button.onClick.AddListener(()=>gameListPanel.OnClickJoinGame(strGameIP));
	}

	private void OnDestroy(){
		Button button = GetComponent<Button>();
		button.onClick.RemoveAllListeners ();
	}

	public void SetGameIPText(string IP){
		strGameIP = IP;
		GameIP.text = strGameIP;
	}
}
