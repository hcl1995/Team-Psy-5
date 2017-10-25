using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack02 : MonoBehaviour
{
	public GameObject impact;

	GameObject impactGO;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			//other.gameObject.GetComponent<Animator>().SetTrigger("DamageDown02");
			other.gameObject.GetComponentInParent<Animator>().SetTrigger("DamageDown02");
			impactGO = (GameObject) Instantiate(impact, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
			Destroy(impactGO, 0.5f);

//			other.transform.LookAt(transform.position);
//
//			Vector3 eulerFucker = other.transform.rotation.eulerAngles;
//			eulerFucker = new Vector3(0, eulerFucker.y, 0);
//			other.transform.rotation = Quaternion.Euler(eulerFucker);
		}
	}
}