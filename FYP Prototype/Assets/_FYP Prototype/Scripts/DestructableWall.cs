using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Projectile"))
		{
			Destroy(gameObject);
		}
	}
}