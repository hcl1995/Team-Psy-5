using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadingScreenScript : MonoBehaviour {

	static public LoadingScreenScript singleton; 
	public Image player1Portrait;
	public Image player2Portrait;
	public Text tutorial;
	public Text Loading;
	public PlayerInfo[] playerSelectCharacter;
	public bool loadReady = false;

	void OnEnable(){
		Loading.text = "LOADING";
		playerSelectCharacter = FindObjectsOfType<PlayerInfo> ();
		foreach (PlayerInfo player in playerSelectCharacter) {
			if (player.playerNumber == 1) {
				player1Portrait.sprite = LobbyController.s_Singleton.selectedCharacterSprite [player.playerCharacter];
			} else if (player.playerNumber == 2) {
				player2Portrait.sprite = LobbyController.s_Singleton.selectedCharacterSprite [player.playerCharacter];
			}
		}
		tutorial.text = strTutorialText (PlayerInfo.singleton.playerCharacter);
		PlayerInfo.singleton.CmdLoadScreenOn ();
	}
	// Use this for initialization
	void Start () {
		singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (loadReady) {
			if (Input.GetKeyUp (KeyCode.Return)) {
				PlayerNetwork.singleton.CmdloadingOnEnterReady ();
				Loading.text = "WAITING FOR YOUR OPPONENT";
			}
		}
	}

	string strTutorialText(int playerCharacter){
		switch (playerCharacter) {
		case 1:
			return "Special Skill 1 : Thai fighter unleashes his magical power. Launch a projectile in a line. Causing enemy to take damage if hit\n\nSpecial Skill 2 : Thai fighter release his inner strength. Granting 2x bonus damage for his next attack\n\nUltimate : Thai fighter connect his magical power with his punch. A single punch that deal ton of damage and apply knock back effect to enemy\n";
		case 2:
			return "Special Skill 1 : Unity Chan summon a tornado that move in a straight line. Causing enemy to take damage if hit\n\nSpecial Skill 2 : Unity Chan release her power. Causing nearby enemy to take damage\n\nUltimate : Unity Chan spin her body to create a powerful tornado kick. A kick that deal ton of damage and apply knock back effect to enemy\n";
		default:
			return "Character not found";
		}
	}


}
