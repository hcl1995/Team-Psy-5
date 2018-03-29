using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour {
	static public StartTimer singleton;
	public Text timerText;

	void Start(){
		singleton = this;
	}

	public void StartMatchTimer(){
		StartCoroutine (StartMatchCountDown ());
	}

	public IEnumerator StartMatchCountDown(){
		timerText.text = "3";
		yield return new WaitForSeconds(1.0f);
		timerText.text = "2";
		yield return new WaitForSeconds(1.0f);
		timerText.text = "1";
		yield return new WaitForSeconds(1.0f);
		timerText.text = "Fight";
		PlayerNetwork.singleton.CmdStartMatchSpawnNow ();
		yield return new WaitForSeconds(1.0f);
		timerText.text = "";

	}
}
