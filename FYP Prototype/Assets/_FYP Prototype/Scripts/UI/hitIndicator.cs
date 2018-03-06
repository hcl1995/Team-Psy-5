using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitIndicator : MonoBehaviour {

	float indicatorDuration = 0.1f;
	float timeElapsed = 0;
	public GameObject Indicator;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (timeElapsed <= indicatorDuration) {
			timeElapsed += Time.deltaTime;
		} else if (timeElapsed >= indicatorDuration) {
			Indicator.SetActive (false);
		}
	}

	public void OnHit(){
		Indicator.SetActive (true);
		timeElapsed = 0;
	}
}
