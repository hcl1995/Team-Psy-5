using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInfo : MonoBehaviour {

	static public LocalPlayerInfo singleton; 
	public int playerNum = 0;
	// Use this for initialization
	void Start () {
		singleton = this;
	}

	public void assginPlayerNumber(int playerNumber){
		playerNum = playerNumber;
	}
}
