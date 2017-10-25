using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentChan : MonoBehaviour
{
	private static OpponentChan _instance;
	public static OpponentChan Instance
	{
		get 
		{
			if(_instance == null)
			{
				GameObject go = GameObject.Find("Opponent");

				_instance = go.GetComponent<OpponentChan>();
				_instance.Start();
			}
			return _instance;
		}
	}

	void Start()
	{
		
	}
}