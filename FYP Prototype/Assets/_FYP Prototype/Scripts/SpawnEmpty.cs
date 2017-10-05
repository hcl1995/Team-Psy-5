using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEmpty : MonoBehaviour
{
	public GameObject empty;

	void Start()
	{
		Instantiate (empty, PlayerControl03.Instance.recordEndPos.position, PlayerControl03.Instance.recordEndPos.rotation);
	}
}