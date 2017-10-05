using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill1Copy : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

	float completionTime;
	public float speed;

	SpawnEmptyCopy spawnEmpty;

	void Start()
	{
		spawnEmpty = GetComponent<SpawnEmptyCopy>();

		lerping = true;

		startPos = transform.position;
		endPos = OnHitChan.Instance.transform.position;
	}

	void Update()
	{
		if (lerping)
		{
			transform.position = Vector3.Lerp(startPos, endPos, completionTime += (Time.deltaTime * speed));
		}
		else
		{
			Destroy(gameObject, 0.5f);
			spawnEmpty.enabled = true;
		}

		if (completionTime >= 0.5)
		{
			lerping = false;
		}

		endPos.y = startPos.y;
	}
}