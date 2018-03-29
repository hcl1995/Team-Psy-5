using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LavaScript : NetworkBehaviour {

	public float damage = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnCollisionStay(Collision other){
//		if (!isServer)
//			return;
//		if (other.gameObject.CompareTag("Player"))
//		{
//			other.gameObject.GetComponent<PlayerHealth> ().takeDamageHazard (damage);
//		}
//	}

	void OnTriggerStay(Collider other){
		if (!isServer)
			return;
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerHealth> ().takeDamageHazard (damage);
		}
	}
}
