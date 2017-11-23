using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitySkill02 : MonoBehaviour
{
	public PlayerHealth selfHealth;
	public PlayerControl03 selfControl;
	public bool isHit = false;
	public float damage;

	void OnEnable(){
		isHit = false;
		StartCoroutine(networkDestroy());
	}

	void OnTriggerEnter(Collider other)
	{
		if (isHit)
			return;
		if (other.gameObject.transform.parent == null)
			return;
		if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth>() != null && selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Wall"))
		{
			if (other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> () != selfHealth) {
				other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeSkill02 (damage,"DamageDown");
				isHit = true;
			}
		}
		Debug.Log(other.gameObject);
	}

//	void OnCollisionEnter(Collision other)
//	{
//		if (isHit)
//			return;
//		if (other.gameObject.CompareTag("Player")) // is there a way to set other side as enemy?
//		{
//			//other.gameObject.GetComponent<Animator>().SetTrigger("Death");
//			other.gameObject.GetComponent<PlayerHealth>().takeSkill02 (damage, "DamageDown");
//			isHit = true;
//		}
//		Debug.Log(other.gameObject);
//	}

	IEnumerator networkDestroy()
	{
		yield return new WaitForSeconds(0.5f);
		gameObject.SetActive(false);
		//NetworkServer.Destroy (gameObject);
	}
}