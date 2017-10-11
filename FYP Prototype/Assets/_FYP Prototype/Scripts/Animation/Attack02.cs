using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack02 : MonoBehaviour
{
	public Transform player;
	public GameObject impact;

	GameObject impactGO;

	void OnTriggerEnter(Collider other)
	{
		Vector3 dir = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position) - transform.position;
		dir = -dir.normalized;

		if (other.gameObject.CompareTag("Enemy"))
		{
			//other.transform.LookAt(player);
//			Quaternion rotation = Quaternion.LookRotation(dir);
//			other.transform.rotation = rotation;
			other.gameObject.GetComponent<Animator>().SetTrigger("OnHit02");
			impactGO = (GameObject) Instantiate(impact, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
			Destroy(impactGO, 0.5f);
		}
	}
}