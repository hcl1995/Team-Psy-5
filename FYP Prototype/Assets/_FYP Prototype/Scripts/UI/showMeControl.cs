using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showMeControl : MonoBehaviour {
	public GameObject control;
	bool isActive = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)){
			if (!isActive) {
				control.SetActive (true);
				isActive = true;
			} else if (isActive) {
				control.SetActive (false);
				isActive = false;
			}
		}
	}
}
