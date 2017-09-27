using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill02 : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

	public float speed;

	void Start()
	{
		lerping = true;

		startPos = transform.position;
		//endPos = transform.position += new Vector3(1.65f, 0, 1.65f);
		endPos = PlayerControl02.Instance.transform.position;
	}

	void Update()
	{
		if (lerping)
		{
			transform.position = Vector3.Lerp(startPos, endPos, speed += Time.deltaTime);
			//transform.position += (new Vector3(1.65f * Time.deltaTime, 0, 1.65f * Time.deltaTime));
		}

		if (speed >= 1)
		{
			lerping = false;
			speed = 0;
		}
	}
}