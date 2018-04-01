using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LavaScript : NetworkBehaviour {
	public float damage = 1.0f;

	void OnTriggerStay(Collider other){
		if (!isServer)
			return;
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerHealth> ().takeDamageHazard (damage);

			if (other.gameObject.GetComponent<SoundEffect>().audioSourceList[1].isPlaying == false)
			{
				other.gameObject.GetComponent<PlayerControl>().CmdPlaySFXClip(8);
				//(other.gameObject.GetComponent<SoundEffect>().selfServiceClip[12]);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (!isServer)
			return;

		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerControl>().CmdStopSFXClip();
		}
	}
}