using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		//transform.position = player.transform.position + offset;
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0.0f, transform.rotation.eulerAngles.z);

	}
}
