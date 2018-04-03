using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : MonoBehaviour {
	static public MatchController singleton;
	public GameObject EndMatch;

	void Start(){
		singleton = this;
	}

	public void OnEndMatch(){
		EndMatch.SetActive (true);
	}
}
