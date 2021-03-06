﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
	public static LevelSelector instance;
	public Image selectedLevel;
	public List<Sprite> levelSprites = new List<Sprite> ();
	public GameObject comfirmButton;
	public GameObject canvas;
	public int selectedStage;
	// Use this for initialization
	void Start () {
		instance = this;
		canvas.SetActive (false);
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
		LoadingScreenCanvas.instance.canvas.sortingOrder = -100;
		if (PlayerInfo.singleton.playerNumber == 1)
			comfirmButton.SetActive (true);
		else
			comfirmButton.SetActive (false);
	}

	public void OffLevelSelect(){
		canvas.SetActive (false);
	}

	public void ChangeSelectedLevelImage(int level){
		selectedStage = level-1;
		selectedLevel.sprite = levelSprites [level - 1];
	}
}
