using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonScript : MonoBehaviour {
	public int level;
	public void OnLevelButtonClick(){
		if(PlayerInfo.singleton.playerNumber==1)
			LevelSelector.instance.OnSelectLevel (level);
	}

}
