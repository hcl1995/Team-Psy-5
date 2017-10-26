using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlToggle : MonoBehaviour
{
	PlayerControl playerControl;
	PlayerControl02 playerControl02;
	PlayerControl03 playerControl03;

	public GameObject controlTxt;
	public GameObject controlTxt02;
	public GameObject controlTxt03;

	void Start()
	{
		playerControl = GetComponent<PlayerControl>();
		playerControl02 = GetComponent<PlayerControl02>();
		playerControl03 = GetComponent<PlayerControl03>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			playerControl.enabled = true;
			playerControl02.enabled = false;
			playerControl03.enabled = false;
			
			controlTxt02.SetActive(false);
			controlTxt03.SetActive(false);
			controlTxt.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			playerControl02.enabled = true;
			playerControl.enabled = false;
			playerControl03.enabled = false;

			controlTxt.SetActive(false);
			controlTxt03.SetActive(false);
			controlTxt02.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			playerControl03.enabled = true;
			playerControl.enabled = false;
			playerControl02.enabled = false;

			controlTxt.SetActive(false);
			controlTxt02.SetActive(false);
			controlTxt03.SetActive(true);
		}
	}
}