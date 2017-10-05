using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEmptyCopy : MonoBehaviour
{
	public GameObject empty;

	void Start()
	{
		Instantiate (empty, AttackChanController.Instance.recordEndPos.position, AttackChanController.Instance.recordEndPos.rotation);
	}
}