using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackUlti : MonoBehaviour
{
	public GameObject impact;
	public PlayerHealth selfHealth; 
	public PlayerControl selfControl;
	public PlayerSkillControl selfPlayerSkill;
	public float distance;
	public bool isHit = false;
//	float timeElapsed = 0;
//	float disableAfter = 1.0f;
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
			if (other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> () != selfHealth && selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Ultimate")) {
				//other.gameObject.GetComponentInParent<Animator> ().SetTrigger ("DamageDown03");
				//Vector3 UltKnockPos = gameObject.transform.root.forward * distance;

				if (selfPlayerSkill.skill02Buffed == false)
				{
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().flying = true;
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().getOtherPos = transform.root.forward * distance;
					other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown03",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position),other);
					//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
					AudioSource.PlayClipAtPoint(selfControl.soundEffect.selfServiceClip[11], other.transform.position, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().CmdInvincible(true);
				}
				else if (selfPlayerSkill.skill02Buffed == true)
				{
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().flying = true;
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().getOtherPos = transform.root.forward * distance;
					other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage * 2,"DamageDown03",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position),other);
					//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
					AudioSource.PlayClipAtPoint(selfControl.soundEffect.selfServiceClip[11], other.transform.position, SoundManager.instance.GetSoundVolume() * SoundManager.instance.GetMasterVolume ());
					other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().CmdInvincible(true);
					selfPlayerSkill.skill02Buffed = false;
				}
				isHit = true;
				//other.gameObject.transform.root.position += (gameObject.transform.root.forward * distance);
//
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
		if (!selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Ultimate")) {
			gameObject.SetActive (false);
		}
	}
}