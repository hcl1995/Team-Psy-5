using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill1 : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

	float completionTime;
	public float speed;

	SpawnEmpty spawnEmpty;

	void Start()
	{
		spawnEmpty = GetComponent<SpawnEmpty>();

		lerping = true;

		startPos = transform.position;
		endPos = OpponentChan.Instance.transform.position;
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