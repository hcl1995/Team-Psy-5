using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructableWall : MonoBehaviour
{
	public float health = 3;

	void Start(){
		
	}
	void OnCollisionEnter(Collision other)
	{
		
		if (other.gameObject.CompareTag("Attack"))
		{
			health -= 1;
			if(health<=0)
				Destroy(gameObject);			
		}
	}

	void OnTriggerEnter(Collider other){
		health -= 1;
		if(health<=0)
			Destroy(gameObject);	
	}
}