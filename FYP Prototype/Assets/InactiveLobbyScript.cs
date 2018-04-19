using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveLobbyScript : MonoBehaviour {
	public bool isPlay = false;
	public float inactiveElapsed = 0.0f;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		inactiveElapsed += Time.deltaTime;
		if (inactiveElapsed >= 900.0f) {
			LobbyController.s_Singleton.OnBackToMainMenu ();

		}
		if(Input.GetAxis("Mouse X")<0){
			//Code for action on mouse moving left
			inactiveElapsed = 0.0f;
		}
		if(Input.GetAxis("Mouse X")>0){
			//Code for action on mouse moving right
			inactiveElapsed = 0.0f;
		}
		if(Input.GetAxis("Mouse Y")<0){
			//Code for action on mouse moving left
			inactiveElapsed = 0.0f;
		}
		if(Input.GetAxis("Mouse Y")>0){
			//Code for action on mouse moving right
			inactiveElapsed = 0.0f;
		}
		if (Input.anyKey) {
			inactiveElapsed = 0.0f;
		}
	}
}
