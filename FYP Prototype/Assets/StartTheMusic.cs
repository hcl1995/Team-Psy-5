using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTheMusic : MonoBehaviour
{
	void Start ()
	{
		SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_INGAME);
		SoundManager.instance.PlaySpecialBGM(BGMAudioClipID.BGM_AMBIENCE);
	}
}