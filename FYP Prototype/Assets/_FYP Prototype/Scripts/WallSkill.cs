using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

	float completionTime = 0.188f;
	public float speed;

	void Start()
	{
		lerping = true;

		startPos = transform.position;
		//endPos = transform.position -= new Vector3(0, 0, 2);
		endPos = PlayerControl02.Instance.transform.position;
	}

	void Update()
	{
		if (lerping)
		{
			transform.position = Vector3.Lerp(startPos, endPos, completionTime += (Time.deltaTime * speed));
			//transform.position -= (new Vector3(0, 0, 4 * Time.deltaTime));
		}

		if (completionTime >= 1.0)
		{
			lerping = false;
			speed = 0;
		}

		if (!lerping)
		{
			Destroy(gameObject, 1.0f);
		}

		endPos.y = startPos.y;
	}
}