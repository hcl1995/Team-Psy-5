using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cinemachine;

public class HealthManager : NetworkBehaviour {

	float player1HealthMax = 200;
	float player2HealthMax = 200;
	[SyncVar(hook = "OnChangeHealth1")]
	float player1HealthCurrent;
	[SyncVar(hook = "OnChangeHealth2")]
	float player2HealthCurrent;
	[SyncVar(hook = "OnPlayer1Character")]
	public int player1Character;
	[SyncVar(hook = "OnPlayer2Character")]
	public int player2Character;


	public int player1Life = 3;
	public int player2Life = 3;

	public RectTransform healthBar1;
	public RectTransform healthBar2;

	public List<GameObject> player1LifeNode = new List<GameObject> ();
	public List<GameObject> player2LifeNode = new List<GameObject> ();


	public Image player1Protrait;
	public Image player2Protrait;
	public List<Sprite> CharacterProtrait = new List<Sprite> ();

	static public HealthManager singleton;

	bool playerDead = false;
	bool player1Dead = false;
	bool player2Dead = false;

	public CinemachineTargetGroup playerGroup;
	public CinemachineVirtualCamera[] manyCamera;

	// Use this for initialization
	void Start () {
		SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_INGAME);

		player1HealthCurrent = player1HealthMax;
		player2HealthCurrent = player2HealthMax;
		singleton = this;
		if (isServer)
			CmdGetProtrait ();
		player1Protrait.sprite = CharacterProtrait [player1Character];
		player2Protrait.sprite = CharacterProtrait [player2Character];
	}

	void Update(){
		player1Protrait.sprite = CharacterProtrait [player1Character];
		player2Protrait.sprite = CharacterProtrait [player2Character];
	}

	public bool takeDamage(int playerNumber,float damage, PlayerControl playerControl){
		if (!isServer)
			return false;
		if (playerNumber == 1) {			
			player1HealthCurrent -= damage*3;
			checkDeath (player1HealthCurrent,player1Life,playerControl,playerNumber);
			if (player1HealthCurrent <= 0)
				return true;
		} else if (playerNumber == 2) {			
			player2HealthCurrent -= damage*3;
			checkDeath (player2HealthCurrent,player2Life,playerControl,playerNumber);
			if (player2HealthCurrent <= 0)
				return true;
		}
		return false;
	}

	void OnChangeHealth1 (float health)
	{
		player1HealthCurrent = health;
		healthBar1.sizeDelta = new Vector2(health, healthBar1.sizeDelta.y);
	}

	void OnChangeHealth2 (float health)
	{
		player2HealthCurrent = health;
		healthBar2.sizeDelta = new Vector2(health, healthBar2.sizeDelta.y);
	}

	public void checkDeath(float health,int life,PlayerControl playerControl,int playerNumber)
	{
		if (health <= 0)
		{
			if (playerDead)
				return;
			if (playerNumber == 1 && !player1Dead) {
				player1Life--;
				player1Dead = true;
				RpcLifeDecrease (playerNumber, player1Life);
				playerControl.playDeathAnim ();
				RpcBrainDead();
				StartCoroutine (respawn (playerNumber, playerControl));

			} else if (playerNumber == 2 && !player2Dead) {
				player2Life--;
				player2Dead = true;
				RpcLifeDecrease (playerNumber, player2Life);
				playerControl.playDeathAnim ();
				RpcBrainDead();
				StartCoroutine (respawn (playerNumber, playerControl));
			}
			if (player2Life <= 0 || player1Life <=0) {
				playerDead = true;
				CmdMatchGame (playerNumber);
				return;
			}

			//CmdAnimation("Death");
			//Debug.Log("Dead!");

		}
	}

	[Command]
	void CmdMatchGame(int playerNumber){
		LobbyController.s_Singleton.checkPlayerConditionNew (playerNumber);
	}

	[ClientRpc]
	void RpcBrainDead()
	{
		playerGroup.enabled = false;
		foreach (CinemachineVirtualCamera vc in manyCamera)
		{
			vc.enabled = false;
		}
	}

	[ClientRpc]
	void RpcBrainAlive()
	{
		playerGroup.enabled = true;
		foreach (CinemachineVirtualCamera vc in manyCamera)
		{
			vc.enabled = true;
		}
	}

	public IEnumerator respawn(int playerNumber, PlayerControl playerControl){
		yield return new WaitForSeconds(3f);
		if (playerNumber == 1) {
			player1HealthCurrent = player1HealthMax;
			player1Dead = false;
		} else if (playerNumber == 2) {
			player2HealthCurrent = player2HealthMax;
			player2Dead = false;
		}
		if(!playerDead)
		{
			playerControl.respawnNow ();
			if (player1Dead == false && player2Dead == false)
				RpcBrainAlive();
		}
		//playerDead = false;
	}

	[Command]
	void CmdGetProtrait(){
		//RpcPlayer1Protrait (LobbyController.s_Singleton.player1CharaterProtrait);
		//RpcPlayer2Protrait (LobbyController.s_Singleton.player2CharaterProtrait);
		player1Character = LobbyController.s_Singleton.player1CharaterProtrait;
		player2Character = LobbyController.s_Singleton.player2CharaterProtrait;
	}

	//[ClientRpc]
	void OnPlayer1Character(int playerCharacter){
		player1Character = playerCharacter;
		player1Protrait.sprite = CharacterProtrait [player1Character];
	}

	//[ClientRpc]
	void OnPlayer2Character(int playerCharacter){
		player2Character = playerCharacter;
		player2Protrait.sprite = CharacterProtrait [player2Character];
	}

	[ClientRpc]
	void RpcLifeDecrease(int playerNumber, int Life){
		if (playerNumber == 1) {
			player1LifeNode [Life].SetActive (false);
		} else if (playerNumber == 2) {
			player2LifeNode [Life].SetActive (false);
		}
	}
}
