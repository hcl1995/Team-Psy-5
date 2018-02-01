using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {
	public const float maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	public float currentHealth = maxHealth;

	public PlayerControl playerControl;
	public int playerNumber;
	public PlayerNetwork pn;
	public Animator anim;
	GameObject impactGO;
	public hitIndicator hitIndicator;
	float previousHealth = maxHealth;


	public RectTransform healthBar;

	bool isDead = false;

	bool isKnockback = false;
	public float completeKnockback;
	public float knockbackAnimationTime;

	void Start(){
		anim = GetComponent<Animator>();
	}
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		currentHealth -= amount;
		checkDeath();
	}

	void Update(){
		if (!isLocalPlayer)
			return;

		if (gameObject.transform.position.y <= -5)
		{
			currentHealth -= maxHealth;
			checkDeath();
		}

		if (currentHealth < previousHealth) {
			hitIndicator.OnHit ();
			previousHealth = currentHealth;
		}

		if (isKnockback)
		{
			completeKnockback += (Time.deltaTime * knockbackAnimationTime);
		}

		if (completeKnockback >= 1)
		{
			isKnockback = false;
			completeKnockback = 0;
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
		if (playerControl.state == PlayerControl.playerState.Guarding) {
			currentHealth -= 1.0f;
		}
		else {
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

		checkDeath();
	}

	public void takeDamageBullet(float damage, string animation){
		if (!isServer)
			return;

//		if (playerControl.skill02Buffed)
//		{
//			currentHealth -= damage * 1.5f;
//			CmdAnimation (animation);
//		}
//		else
//		{
			currentHealth -= damage;
			CmdAnimation (animation);
//		}
		//CmdHit ();

		checkDeath();
	}

	public void takeSkill02(float damage, string animation){
		if (!isServer)
			return;

		currentHealth -= damage;
		CmdAnimation (animation);

		checkDeath();
	}

	public void takeDamageHazard(float damage){
		if (!isServer)
			return;

		currentHealth -= damage;
		//CmdHit ();
		checkDeath();
	}
	public void takeMuaiThaiUlt(float damage, string animation, GameObject impact, Vector3 position, Vector3 euler,Vector3 colliderHit,Collider other,Vector3 KnockPos){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Guarding) {
			currentHealth -= 1.0f;
		}
		else {
			currentHealth -= damage;
			CmdKnockBack (KnockPos);
			CmdAnimation (animation);
		}
		//CmdHit ();

		isKnockback = true;

		impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
		NetworkServer.Spawn (impactGO);
		Destroy (impactGO, 0.5f);
		transform.root.LookAt (position);
		Vector3 eulerFucker = euler;
		eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
		transform.root.rotation = Quaternion.Euler (eulerFucker);

		//currPosition += (transform.root.forward * distance);
		//other.transform.root.position = Vector3.Lerp (other.transform.root.position, KnockPos, completeKnockback);

		checkDeath();
	}
	public void checkDeath()
	{
		if (currentHealth <= 0)
		{
			if (isDead)
				return;
			currentHealth = 0;
			isDead = true;
			CmdAnimation("Death");
			//Debug.Log("Dead!");
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
	[Command]
	void CmdKnockBack(Vector3 pos){
		RpcKnockBack (pos);
	}
	[ClientRpc]
	void RpcKnockBack(Vector3 pos){
		gameObject.transform.root.position += pos;
		//gameObject.transform.root.position = Vector3.Lerp (gameObject.transform.root.position, pos, completeKnockback);
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
