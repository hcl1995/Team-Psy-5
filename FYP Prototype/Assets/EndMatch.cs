using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMatch : MonoBehaviour {
	static public EndMatch singleton;
	public Image endMatchBackground;
	public GameObject Player1;
	public Text Player1Text;
	public GameObject Player2;
	public Text Player2Text;
	public float alphaColor = 0;
	public float alphaIncreaseRate = 0;
	public bool isAlphaMax = false;
	public Text text;
	bool isReady = false;
	public GameObject playerAgain;
	public Image loser1;
	public Image loser2;
	public List<Sprite> loserSprite = new List<Sprite> ();
	public List<Sprite> WinnerSprite = new List<Sprite> ();
	// Use this for initialization
	void Start () {
		singleton = this;
	}
	void Update(){
		if (!isAlphaMax) {
			alphaColor += Time.deltaTime * alphaIncreaseRate;
			endMatchBackground.color = new Color(1,1,1,alphaColor);
			if (alphaColor >= 0.8f) {
				isAlphaMax = true;
				if (HealthManager.singleton.WinPlayerInt == 1) {
					Player1.SetActive (true);
					Player1.GetComponent<Image> ().sprite = WinnerSprite [HealthManager.singleton.player1Character];
					Player1Text.text = getWinnerText (HealthManager.singleton.player1Character);
					loser1.sprite = loserSprite [HealthManager.singleton.player2Character];
					loser1.SetNativeSize ();
				} else if (HealthManager.singleton.WinPlayerInt == 2) {
					Player2.SetActive (true);
					Player2.GetComponent<Image> ().sprite = WinnerSprite [HealthManager.singleton.player2Character];
					Player2Text.text = getWinnerText (HealthManager.singleton.player2Character);
					loser2.sprite = loserSprite [HealthManager.singleton.player1Character];
					loser2.SetNativeSize ();
				}
				text.text = "PRESS ANY KEY TO CONTINUE";
			}
		}
		if (isAlphaMax) {
			if (Input.anyKeyDown && !isReady) {
				//PlayerNetwork.singleton.CmdOnEndMatchReady ();
				playerAgain.SetActive(true);
				isReady = true;
			}
		}
	}

	string getWinnerText(int winnerTextInt){
		if (winnerTextInt == 1) {
			int rand = Random.Range (0, 3);
			switch (rand) {
			case 0:
				return "Ha! Not even close";
			case 1:
				return "Behold the power of darkness";
			case 2:
				return "A little bit of punch and a little bit of kick. EASY!!";
			default:
				return "Something not right";
			}
		} else if (winnerTextInt == 2) {
			int rand = Random.Range (0, 2);
			switch (rand) {
			case 0:
				return "Ha! How does the wind taste like";
			case 1:
				return "Warm up done…Oh! I win yeah";
			default:
				return "Something not right";
			}
		}
		return "Something not right";
	}

	public void OnYesPlayAgain(){
		PlayerNetwork.singleton.CmdOnEndMatchReady ();
		text.text = "WAITING FOR YOUR OPPONENT";
		playerAgain.SetActive(false);
	}

	public void OnNoPlayAgain(){
		LobbyController.s_Singleton.OnBackToLobbyMenu ();
		playerAgain.SetActive(false);
	}
}
