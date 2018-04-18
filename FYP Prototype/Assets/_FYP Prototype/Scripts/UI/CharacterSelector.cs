using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelector : MonoBehaviour {
	public int intCharacter;
	public PlayerInfo playerInfo;
	public GameObject selectBorder1Main;
	public GameObject selectBorder1Sub;
	public GameObject selectBorder2Main;
	public GameObject selectBorder2Sub;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInfo.player1Select == intCharacter) {
			selectBorder1Main.SetActive (true);
			selectBorder1Sub.SetActive (true);
		} 
		if (playerInfo.player2Select == intCharacter) {
			selectBorder2Main.SetActive (true);
			selectBorder2Sub.SetActive (true);
		}  
		if (playerInfo.player1Select != intCharacter) {
			selectBorder1Main.SetActive (false);
			selectBorder1Sub.SetActive (false);
		} 
		if (playerInfo.player2Select != intCharacter) {
			selectBorder2Main.SetActive (false);
			selectBorder2Sub.SetActive (false);
		}
	}

	public void OnButtonClick(){
		playerInfo.setSelectCharacter(intCharacter);
	}

}
