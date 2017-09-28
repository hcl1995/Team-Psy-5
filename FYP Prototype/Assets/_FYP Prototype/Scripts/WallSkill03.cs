using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSkill03 : MonoBehaviour
{
	Vector3 startPos;
	Vector3 endPos;

	bool lerping;

	float completionTime;
	public float speed;

	void Start()
	{
		lerping = true;

		startPos = transform.position;
		//endPos = new Vector3(Mathf.Abs (transform.position.x - 1.65f), 0, transform.position.z + 1.65f);
		endPos = PlayerControl02.Instance.transform.position; // + some value
	}

	void Update()
	{
		if (lerping)
		{
			transform.position = Vector3.Lerp(startPos, endPos, completionTime += (Time.deltaTime * speed));
			//transform.position -= new Vector3(1.65f * Time.deltaTime, 0, 0);
			//transform.position += new Vector3(0, 0, 1.65f * Time.deltaTime);
		}

		if (completionTime >= 0.812)
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