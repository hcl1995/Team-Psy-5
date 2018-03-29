using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanUltimate : MonoBehaviour
{
	public GameObject impact;
	public PlayerHealth selfHealth; 
	public PlayerControl selfControl;

	public float distance;
	public bool isHit = false;

	public float damage;

	//public int hitCount = 5;
	//public float hitInterval;
	//public float curInterval;

	void OnEnable(){
		isHit = false;

		//hitCount = 5;
		//curInterval = 0;
	}

	void OnTriggerEnter(Collider other)
	{
		if (isHit)
			return;
		if (other.gameObject.transform.root == null)
			return;
		if (other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth>() != null)
		{
			if (other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> () != selfHealth)// && selfControl.animation.GetCurrentAnimatorStateInfo (0).IsName ("Ultimate"))
			{
//				if (hitCount >= 0)
//				{
//					curInterval += Time.deltaTime;
//				}
				Vector3 UltKnockPos = new Vector3(0, 0, 0);

				//if (curInterval >= hitInterval && hitCount >= 0)
				//{
//					curInterval = 0;
//					hitCount--;
//
//					other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position),other,UltKnockPos);
//
//					if (hitCount == 0)
//					{
//						UltKnockPos = gameObject.transform.root.forward * distance;
//						other.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown03",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider> ().ClosestPointOnBounds (transform.position),other,UltKnockPos);
//					}
				//}

				Vector3 randPos = new Vector3 (0, Random.Range(other.transform.position.y - 1, other.transform.position.y + 1), 0);

				for (int i = 1; i <= 5; i++)
				{
					if (i < 5)
					{
						other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown02",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(randPos),other);
						//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
						AudioSource.PlayClipAtPoint(selfControl.soundEffect.selfServiceClip[11], other.transform.position);
					}
					else if (i == 5)
					{
						other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().flying = true;
						other.gameObject.transform.root.gameObject.GetComponent<PlayerControl>().flyDistance = distance;

						UltKnockPos = gameObject.transform.root.forward * distance;
						other.gameObject.transform.root.gameObject.GetComponent<PlayerHealth> ().takeMuaiThaiUlt (damage,"DamageDown03",impact,transform.position,other.transform.rotation.eulerAngles,other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position),other);
						//AudioSource.PlayClipAtPoint(SoundManager.instance.onHitClip, other.transform.position);
						AudioSource.PlayClipAtPoint(selfControl.soundEffect.selfServiceClip[11], other.transform.position);
					}
				}
				isHit = true;
			}
		}
	}

	void Update(){
		//if (isHit)
		//{
//			if (hitCount >= 0)
//			{
//				curInterval += Time.deltaTime;
//			}
		//}
	}
}