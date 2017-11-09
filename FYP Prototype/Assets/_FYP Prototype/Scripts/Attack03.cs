using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack03 : MonoBehaviour
{
	public GameObject impact;

	public float distance;

	GameObject impactGO;

//	Vector3 startPos;
//	Vector3 endPos;
//
//	bool sendFlying = false;
//	public float completeTime;
//	float lerpSpeed = 5;

//	void Update()
//	{
//		if (sendFlying)
//		{
//			completeTime += (Time.deltaTime * lerpSpeed);
//			transform.position = Vector3.Lerp (startPos, endPos, completeTime);
//		}
//
//		if (completeTime >= 1)
//		{
//			sendFlying = false;
//			completeTime = 0;
//		}
//	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponentInParent<Animator>().SetTrigger("DamageDown03");
			other.gameObject.transform.root.position += (gameObject.transform.root.forward * distance);

			impactGO = (GameObject) Instantiate(impact, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
			Destroy(impactGO, 0.5f);

			other.transform.root.LookAt(transform.position);

			Vector3 eulerFucker = other.transform.rotation.eulerAngles;
			eulerFucker = new Vector3(0, eulerFucker.y - 180f, 0);
			other.transform.root.rotation = Quaternion.Euler(eulerFucker);
		}
	}
}