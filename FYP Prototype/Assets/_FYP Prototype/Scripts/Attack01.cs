using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack01 : MonoBehaviour
{
	public GameObject impact;

	GameObject impactGO;

	// ONTRIGGER NOT WORKING, BULLET WORKS WELL...? DEFUQ

	// HOW THE FUCK AM I NOT HITTING TWICE?
	void OnTriggerEnter(Collider other)
	{
//		Vector3 dir = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position) - transform.position;
//		dir = -dir.normalized;

		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponentInParent<Animator>().SetTrigger("DamageDown");
			impactGO = (GameObject) Instantiate(impact, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
			Destroy(impactGO, 0.5f);

			other.transform.root.LookAt(transform.position);

			Vector3 eulerFucker = other.transform.rotation.eulerAngles;
			eulerFucker = new Vector3(0, eulerFucker.y - 180f, 0);
			other.transform.root.rotation = Quaternion.Euler(eulerFucker);

			// Some Multiple Punches
			//Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), true);
		}
	}

//	void OnTriggerExit(Collider other)
//	{
//		if (other.gameObject.CompareTag("Enemy"))
//		{
//			Physics.IgnoreCollision(GetComponent<Collider>(), other.GetComponent<Collider>(), false);
//		}
//	}
}