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
			other.gameObject.GetComponent<PlayerControl>().CmdLastResort();
			other.gameObject.GetComponent<Animator>().SetBool("LegPainBool", true);

			other.gameObject.GetComponent<PlayerHealth> ().takeDamageHazard (damage); // Trigger doesn't looks that bad

			if (other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName ("LegPain") == false)
			{
				other.gameObject.GetComponent<PlayerHealth>().legPainAnimation("LegPain");
			}

			if (other.gameObject.GetComponent<PlayerHealth>().isDead == true)//(other.gameObject.GetComponent<PlayerControl>().state == PlayerControl.playerState.Death)
			{
				other.gameObject.GetComponent<PlayerControl>().CmdStopSFXClip();
			}
			else if (other.gameObject.GetComponent<SoundEffect>().audioSourceList[1].isPlaying == false && 
				other.gameObject.GetComponent<PlayerHealth>().isDead == false)
			{
				other.gameObject.GetComponent<PlayerControl>().CmdPlaySFXClip(8);
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