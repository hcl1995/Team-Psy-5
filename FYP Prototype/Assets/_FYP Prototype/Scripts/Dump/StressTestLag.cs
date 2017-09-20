using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTestLag : MonoBehaviour
{
	bool spawn = true;
	public GameObject unityChan;
	public List <GameObject> unityChuan = new List<GameObject>();

	void Update ()
	{
		if (spawn)
		{
			Instantiate(unityChan, new Vector3(Random.Range(-12.5f, 12.5f), 0, Random.Range(-12.5f, 12.5f)), Quaternion.identity);
			unityChuan.Add(unityChan);
		}

		if (unityChuan.Count >= 150)
		{
			spawn = false;
		}

		// One UnityChan estimate 80K Tris.
		// 250 = 35 - 37fps // assume 250 is definitely not safe (Tris - 10.6M).
		// 200 = 42 - 45fps // 200 isn't safe either exclude the factors affecting frame rate (Tris - 7.8M).
	}
}