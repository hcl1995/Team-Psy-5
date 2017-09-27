using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

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
			transform.position = Vector3.Lerp(startPos, endPos, speed += Time.deltaTime);
			//transform.position -= (new Vector3(0, 0, 4 * Time.deltaTime));
		}

		if (speed >= 1)
		{
			lerping = false;
			speed = 0;
		}
	}
}