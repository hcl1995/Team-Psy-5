using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchListing : MonoBehaviour {

	public Button button;

	[SerializeField]
	private Text _gameIP;
	public Text GameIP{
		get{ return _gameIP; }
	}

	public string strGameIP{ get; private set;}
	public bool Updated{ get; set; }

	private void Start(){
		GameObject gameListPanelObj = LobbyController.s_Singleton.FindMatchPanel.gameObject;//transform.parent.GetComponent<GameListPanel> ().gameObject;
		if (gameListPanelObj == null)
			return;

		FindMatchPanel findMatchPanel = gameListPanelObj.GetComponent<FindMatchPanel> ();
		button.onClick.AddListener(()=>findMatchPanel.OnClickJoinGame(strGameIP));
	}

	private void OnDestroy(){
		button.onClick.RemoveAllListeners ();
	}

	public void SetGameIPText(string IP){
		strGameIP = IP;
		GameIP.text = strGameIP;
	}
}
