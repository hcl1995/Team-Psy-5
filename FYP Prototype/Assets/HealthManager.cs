using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthManager : NetworkBehaviour {

	float player1HealthMax = 200;
	float player2HealthMax = 200;
	[SyncVar(hook = "OnChangeHealth1")]
	float player1HealthCurrent;
	[SyncVar(hook = "OnChangeHealth2")]
	float player2HealthCurrent;

	int player1Life = 1;
	int player2Life = 1;

	public RectTransform healthBar1;
	public RectTransform healthBar2;

	static public HealthManager singleton;

	bool playerDead = false;

	// Use this for initialization
	void Start () {
		player1HealthCurrent = player1HealthMax;
		player2HealthCurrent = player2HealthMax;
		singleton = this;
	}
	
	public void takeDamage(int playerNumber,float damage, PlayerControl playerControl){
		if (!isServer)
			return;
		if (playerNumber == 1) {			
			player1HealthCurrent -= damage*3;
			checkDeath (player1HealthCurrent,player1Life,playerControl,playerNumber);
		} else if (playerNumber == 2) {			
			player2HealthCurrent -= damage*3;
			checkDeath (player2HealthCurrent,player2Life,playerControl,playerNumber);
		}

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
			if (life <= 0) {
				playerDead = true;
				CmdMatchGame (playerNumber);
				return;
			}
			if (playerNumber == 1) {
				player1HealthCurrent = player1HealthMax;
				player1Life--;
			} else if (playerNumber == 2) {
				player2HealthCurrent = player2HealthMax;
				player2Life--;
			}
			playerControl.respawnNow ();
			//CmdAnimation("Death");
			//Debug.Log("Dead!");

		}
	}

	[Command]
	void CmdMatchGame(int playerNumber){
		LobbyController.s_Singleton.checkPlayerConditionNew (playerNumber);
	}


}
