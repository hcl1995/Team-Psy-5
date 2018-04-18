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

	[SyncVar]
	public int WinPlayerInt = 0;

	public Image healthBar1;
	public Image healthBar2;

	public List<GameObject> player1LifeNode = new List<GameObject> ();
	public List<GameObject> player2LifeNode = new List<GameObject> ();


	public Image player1Protrait;
	public Image player2Protrait;
	public Image Player1Name;
	public Image Player2Name;
	public List<Sprite> CharacterProtrait = new List<Sprite> ();
	public List<Sprite> CharacterName = new List<Sprite> ();
	public Sprite lifeGone1;
	public Sprite lifeGone2;

	static public HealthManager singleton;

	bool playerDead = false;
	bool player1Dead = false;
	bool player2Dead = false;

	public CinemachineTargetGroup playerGroup;
	public CinemachineVirtualCamera[] manyCamera;

	// Use this for initialization
	void Start () {
		player1HealthCurrent = player1HealthMax;
		player2HealthCurrent = player2HealthMax;
		singleton = this;
		if (isServer)
			CmdGetProtrait ();
	}

	void Update(){
		player1Protrait.sprite = CharacterProtrait [player1Character];
		Player1Name.sprite = CharacterName [player1Character];
		player2Protrait.sprite = CharacterProtrait [player2Character];
		Player2Name.sprite = CharacterName [player2Character+2];
	}

	public void takeDamage(int playerNumber,float damage, PlayerControl playerControl){
		if (playerDead)
			return;
		if (!isServer)
			return;
		if (playerNumber == 1) {	
			if (player1HealthCurrent <= 0)
				return;
			player1HealthCurrent -= damage;
			checkDeath (player1HealthCurrent,player1Life,playerControl,playerNumber);
		} else if (playerNumber == 2) {
			if (player2HealthCurrent <= 0)
				return;
			player2HealthCurrent -= damage;
			checkDeath (player2HealthCurrent,player2Life,playerControl,playerNumber);

		}
	}

	void OnChangeHealth1 (float health)
	{
		player1HealthCurrent = health;
		healthBar1.fillAmount = player1HealthCurrent / player1HealthMax;
		if (LocalPlayerInfo.singleton.playerNum == 1) {
			LocalPlayerInfo.singleton.OnHit ();
		}
	}

	void OnChangeHealth2 (float health)
	{
		player2HealthCurrent = health;
		healthBar2.fillAmount = player2HealthCurrent / player2HealthMax;
		if (LocalPlayerInfo.singleton.playerNum == 2) {
			LocalPlayerInfo.singleton.OnHit ();
		}
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
				if (playerNumber == 1) {
					WinPlayerInt = 2;
				} else if (playerNumber == 2) {
					WinPlayerInt = 1;
				}
				RpcEndGame ();
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
		if(!playerDead)
		{
			if (playerNumber == 1) {
				player1HealthCurrent = player1HealthMax;
				player1Dead = false;
			} else if (playerNumber == 2) {
				player2HealthCurrent = player2HealthMax;
				player2Dead = false;
			}
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
			player1LifeNode [Life].GetComponent<Image>().sprite = lifeGone1;
		} else if (playerNumber == 2) {
			player2LifeNode [Life].GetComponent<Image>().sprite = lifeGone2;
		}
	}

	[ClientRpc]
	void RpcPlayThis(){
		SoundManager.instance.PlayBGM(BGMAudioClipID.BGM_IMMORTALSELECTION);
	}

	[ClientRpc]
	void RpcEndGame(){
		MatchController.singleton.EndMatch.SetActive (true);
	}
}
