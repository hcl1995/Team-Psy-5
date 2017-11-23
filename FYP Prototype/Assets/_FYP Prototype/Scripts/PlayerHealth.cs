using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {
	public const float maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	public float currentHealth = maxHealth;

	public PlayerControl03 playerControl;
	public int playerNumber;
	public PlayerNetwork pn;
	public Animator anim;
	GameObject impactGO;
	public hitIndicator hitIndicator;
	float previousHealth = maxHealth;


	public RectTransform healthBar;

	void Start(){
		anim = GetComponent<Animator>();
	}
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
		}
	}

	void Update(){
		if (!isLocalPlayer)
			return;
		if (currentHealth < previousHealth) {
			hitIndicator.OnHit ();
			previousHealth = currentHealth;
		}
	}

	void OnChangeHealth (float health)
	{
		currentHealth = health;
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	public void takeDamage(float damage, string animation, GameObject impact, Vector3 position, Vector3 euler,Vector3 colliderHit){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl03.playerState.Guarding) {
			currentHealth -= 1.0f;
		} else {
			currentHealth -= damage;
			CmdAnimation (animation);
		}
		//CmdHit ();

		impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
		NetworkServer.Spawn (impactGO);
		Destroy (impactGO, 0.5f);
		transform.root.LookAt (position);
		Vector3 eulerFucker = euler;
		eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
		transform.root.rotation = Quaternion.Euler (eulerFucker);

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
			CmdLose ();
		}
	}

	public void takeDamageBullet(float damage, string animation){
		if (!isServer)
			return;

		if (playerControl.state == PlayerControl03.playerState.Guarding) {
			currentHealth -= 1.0f;
		} else {
			currentHealth -= damage;
			CmdAnimation (animation);
		}
		//CmdHit ();

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
			CmdLose ();
		}
	}

	public void takeSkill02(float damage, string animation){
		if (!isServer)
			return;

		currentHealth -= damage;
		CmdAnimation (animation);

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
			CmdLose ();
		}
	}

//	public void sei9jor()
//	{
//		if (currentHealth <= 0)
//		{
//			currentHealth = 0;
//			CmdAnimation ("Death");
//			Debug.Log("Dead!");
//			CmdLose ();
//		}
//	}

	public void takeDamageHazard(float damage){
		if (!isServer)
			return;

		currentHealth -= damage;
		//CmdHit ();

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
			CmdLose ();
		}
	}

	[Command]
	public void CmdLose(){
		LobbyController.s_Singleton.checkPlayerCondition ();
	}

	void OnDestroy(){
		NetworkServer.Destroy (impactGO);
	}

	[Command]
	public void CmdAnimation(string anim){
		RpcAmintion (anim);
	}

	[ClientRpc]
	public void RpcAmintion(string animation){
		anim.SetTrigger (animation);
	}

//	[Command]
//	public void CmdHit(){
//		RpcShowHitIndicator ();
//	}
//
//	[ClientRpc]
//	public void RpcShowHitIndicator(){
//		hitIndicator.SetActive (true);
//	}
}
