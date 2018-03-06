using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackKb : MonoBehaviour
{
	public GameObject impact;
	public PlayerHealth selfHealth; 
	public PlayerControl selfControl;
	public float distance;
	public bool isHit = false;
	float timeElapsed = 0;
	float disableAfter = 1.0f;
	public float damage;

	GameObject impactGO;

	void Awake(){
		Debug.Log ("Awake");
	}
	void Start(){
		Debug.Log ("Start");
	}
	void OnEnable(){
		Debug.Log ("OnEnable");
		Debug.Log ("3");
		isHit = false;
	}
	void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.gameObject);
		if (isHit)
			return;
		if (other.gameObject.transform.root == null)
			return;
		if (other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth>() != null)
		{
			if (other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> () != selfHealth) {
				//other.gameObject.GetComponentInParent<Animator> ().SetTrigger ("DamageDown03");
				Vector3 knockPos = gameObject.transform.root.forward * distance;

				other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown03",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position), other, knockPos);
				//other.transform.root.position += (transform.root.forward * distance);

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

//	void Update(){
////		if(isHit)
////			gameObject.SetActive (false);
////		if ((int)selfControl.state != (int)PlayerControl03.playerState.Attacking) {
////			gameObject.SetActive (false);
////		}
//		if (!selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack03")) {
//			gameObject.SetActive (false);
//		}
//	}
}