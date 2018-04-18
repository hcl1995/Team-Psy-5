using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnTheMusic : MonoBehaviour
{
	void Start () {
		SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_THEDUEL);
	}
}