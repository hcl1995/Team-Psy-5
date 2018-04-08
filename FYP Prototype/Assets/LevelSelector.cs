using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
	public static LevelSelector instance;
	public Image selectedLevel;
	public List<Sprite> levelSprites = new List<Sprite> ();
	public GameObject comfirmButton;
	public GameObject canvas;
	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void OnSelectLevel(int level){
		PlayerInfo.singleton.CmdSelectLevel (level);
	}

	public void OnComfirm(){
		PlayerInfo.singleton.RpcEnableLoading ();
	}

	public void OnReadyLevelSelect(){
		canvas.SetActive (true);
		PlayerInfo.singleton.DisableCharacterSelector ();
		if (PlayerInfo.singleton.playerNumber == 1)
			comfirmButton.SetActive (true);
	}

	public void OffLevelSelect(){
		canvas.SetActive (false);
	}

	public void ChangeSelectedLevelImage(int level){
		selectedLevel.sprite = levelSprites [level - 1];
	}
}
