using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelector : MonoBehaviour {
	public int intCharacter;
	public PlayerInfo playerInfo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonClick(){
		playerInfo.setSelectCharacter(intCharacter);
	}

}
