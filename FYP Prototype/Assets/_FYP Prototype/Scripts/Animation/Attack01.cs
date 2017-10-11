using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack01 : MonoBehaviour
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
			other.gameObject.GetComponent<Animator>().SetTrigger("OnHit01");
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