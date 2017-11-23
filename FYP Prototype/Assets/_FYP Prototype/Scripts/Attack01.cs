using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Attack01 : MonoBehaviour
{
	public GameObject impact;
	public PlayerHealth selfHealth; 
	public PlayerControl03 selfControl;
	public bool isHit = false;
	GameObject impactGO;
	public float damage;

	// ONTRIGGER NOT WORKING, BULLET WORKS WELL...? DEFUQ

	void Awake(){
		Debug.Log ("Awake");
	}
	void Start(){
		Debug.Log ("Start");
	}
	void OnEnable(){
		Debug.Log ("OnEnable");
		Debug.Log ("1");
		isHit = false;
	}

	void OnTriggerEnter(Collider other)
	{
//		Vector3 dir = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position) - transform.position;
//		dir = -dir.normalized;
		Debug.Log (other.gameObject);
		if (other.gameObject.transform.parent == null)
			return;
		if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth>() != null)
		{
			if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> () != selfHealth) {
				//other.gameObject.GetComponentInParent<Animator> ().SetTrigger ("DamageDown");
				other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage,"DamageDown",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
				isHit = true;
//				impactGO = (GameObject)Instantiate (impact, other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position), Quaternion.identity);
//				Destroy (impactGO, 0.5f);
//
//				other.transform.root.LookAt (transform.position);
//
//				Vector3 eulerFucker = other.transform.rotation.eulerAngles;
//				eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
//				other.transform.root.rotation = Quaternion.Euler (eulerFucker);
			}
		}
	}
	void Update(){
//		if(isHit)
//			gameObject.SetActive (false);
//		if ((int)selfControl.state != (int)PlayerControl03.playerState.Attacking) {
//			gameObject.SetActive (false);
//		}
//		if (!selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {
//			gameObject.SetActive (false);
//		}
	}
}