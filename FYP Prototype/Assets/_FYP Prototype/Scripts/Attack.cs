using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Attack : MonoBehaviour
{
	public GameObject impact;
	public PlayerHealth selfHealth;
	public PlayerControl selfControl;
	public PlayerSkillControl selfPlayerSkill;
	public bool isHit = false;
	GameObject impactGO;
	public float damage;

	void Awake(){
		Debug.Log ("Awake");
	}
	void Start(){
		Debug.Log ("Start");
	}
	void OnEnable(){
		Debug.Log ("OnEnable");
		Debug.Log ("2");

		isHit = false;
	}
	void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.gameObject);
		if (isHit)
			return;
		if (other.gameObject.transform.parent == null)
			return;
		if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth>() != null && (selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack02")||selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack")))
		{
			if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> () != selfHealth) {
				if (selfPlayerSkill == null)
				{
					other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
				}
				else if (selfPlayerSkill != null)
				{
					if (selfPlayerSkill.skill02Buffed == false)
					{
						other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
					}
					else if (selfPlayerSkill.skill02Buffed == true)
					{
						other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage * 1.5f,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
						selfPlayerSkill.skill02Buffed = false;
					}
				}
//				else if (selfPlayerSkill.skill02Buffed == false)
//				{
//					other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
//				}
//				else if (selfPlayerSkill.skill02Buffed == true)
//				{
//					other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeDamage (damage * 1.5f,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position));
//					selfPlayerSkill.skill02Buffed = false;
//				}
				isHit = true;
//				impactGO = (GameObject) Instantiate(impact, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
//				Destroy(impactGO, 0.5f);
//
//				other.transform.root.LookAt(transform.position);
//
//				Vector3 eulerFucker = other.transform.rotation.eulerAngles;
//				eulerFucker = new Vector3(0, eulerFucker.y - 180f, 0);
//				other.transform.root.rotation = Quaternion.Euler(eulerFucker);
			}
		}
	}

//	void Update(){
////		if(isHit)
////			gameObject.SetActive (false);
////		if ((int)selfControl.state != (int)PlayerControl03.playerState.Attacking) {
////			gameObject.SetActive (false);
////		}
//		if (!selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack02") && !selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {
//			gameObject.SetActive (false);
//		}
//	}
}