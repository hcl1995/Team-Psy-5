using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanelScript : MonoBehaviour {
	public Text IpAddress;
	public string str;
	// Use this for initialization
	void OnEnable(){
		IpAddress.text = "IP ADDRESS: " + Network.player.ipAddress;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		str = Network.player.ipAddress;
	}
}
