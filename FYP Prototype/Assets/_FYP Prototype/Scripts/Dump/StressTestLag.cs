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

		if (unityChuan.Count >= 100)
		{
			spawn = false;
		}

		// One UnityChan estimate 80K Tris.
		// 250 = 33 - 37fps // assume 250 is definitely not safe (Tris - 10.6M).
		// 200 = 43 - 47fps // 200 isn't safe either exclude the factors affecting frame rate (Tris - 7.8M).
		// 150 = 6x fps (Tris - 6.1M) considering 50% as the budget of hardware & art factors 75 game object is the limit.
		// 075 = (Tris - 3.3M) #Conclude
		// When change camera setup - Tris count will change as well.
	}
}