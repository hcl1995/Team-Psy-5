using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHitChan : MonoBehaviour
{
	private static OnHitChan _instance;
	public static OnHitChan Instance
	{
		get 
		{
			if(_instance == null)
			{
				GameObject go = GameObject.Find("Onhit_1");

				_instance = go.GetComponent<OnHitChan>();
				_instance.Start();
			}
			return _instance;
		}
	}

	void Start()
	{
		
	}
}