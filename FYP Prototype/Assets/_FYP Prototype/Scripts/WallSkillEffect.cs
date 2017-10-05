using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkillEffect : MonoBehaviour
{
	void Start()
	{
		Destroy (gameObject, 0.5f);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<Animator>().SetTrigger("DamageDown");
			// disable opponent movements, player control scripts.
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Opponent Stunned.");
	}
}