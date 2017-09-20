using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlToggle : MonoBehaviour
{
	PlayerControl playerControl;
	PlayerControl02 playerControl02;

	public GameObject controlTxt;
	public GameObject controlTxt02;

	void Start()
	{
		playerControl = GetComponent<PlayerControl>();
		playerControl02 = GetComponent<PlayerControl02>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			playerControl.enabled = true;
			playerControl02.enabled = false;
			
			controlTxt02.SetActive(false);
			controlTxt.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			playerControl02.enabled = true;
			playerControl.enabled = false;

			controlTxt.SetActive(false);
			controlTxt02.SetActive(true);
		}
	}
}