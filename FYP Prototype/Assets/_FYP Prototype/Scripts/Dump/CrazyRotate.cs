using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyRotate : MonoBehaviour
{
	public float speed = 1000;

	void Update ()
	{
		transform.Rotate (Vector3.up * (Time.deltaTime * speed));
	}
}