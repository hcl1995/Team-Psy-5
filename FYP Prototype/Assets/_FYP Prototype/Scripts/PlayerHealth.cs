using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {
	public const float maxHealth = 100;

	public float currentHealth = maxHealth;

	public PlayerControl playerControl;
	public PlayerSkillControl playerSkillControl;
	public int playerNumber;
	public PlayerNetwork pn;
	public Animator anim;
	GameObject impactGO;
	public GameObject superGuard;
	public ParticleSystem m_SuperGuard;
	public hitIndicator hitIndicator;
	float previousHealth = maxHealth;
	public bool harzardDamageCD = false;
	public float harzardDamageCDDuration = 1.0f;
	public float harzardDamageCDElapsed = 0.0f;
	public bool falled = false;
	public float fallElapsed = 0.0f;

	bool isDead = false;

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

		if (gameObject.transform.position.y <= -0.5)
		{
			//if (!falled) {
				CmdFallToDeath ();
//				falled = true;
//			}
//			fallElapsed += Time.deltaTime;
//			if (fallElapsed >= 10.5f) {
//				falled = false;
//				fallElapsed = 0.0f;
//			}
		}

//		if (currentHealth < previousHealth) {
//			hitIndicator.OnHit ();
//			previousHealth = currentHealth;
//		}



	}

	public IEnumerator hazardCoolDown(){
		yield return new WaitForSeconds (harzardDamageCDDuration);
		harzardDamageCD = false;
		harzardDamageCDElapsed = 0.0f;
	}

	void OnChangeHealth (float health)
	{
		currentHealth = health;
	}

	public void takeDamage(float damage, string animation, GameObject impact, Vector3 position, Vector3 euler,Vector3 colliderHit, GameObject guardImpact){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		if (playerControl.state == PlayerControl.playerState.Guarding) {
			HealthManager.singleton.takeDamage (playerNumber, 1.0f,playerControl);
			playerControl.CmdPlaySFXClip(6);

			CmdGuardParticle();
			//m_SuperGuard.Play();
			//impactGO =  (GameObject)Instantiate (superGuard, position, Quaternion.identity);
			//NetworkServer.Spawn (impactGO);
			//Destroy (impactGO, 1.5f);
		}
		else {
			HealthManager.singleton.takeDamage (playerNumber, damage,playerControl);
			CmdAnimation (animation);

			impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
			NetworkServer.Spawn (impactGO);
			Destroy (impactGO, 1f);
		}
		//CmdHit ();

		// doesn't seems working here.
//		transform.root.LookAt (colliderHit);
//		Vector3 eulerFucker = euler;
//		eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
//		transform.root.rotation = Quaternion.Euler (eulerFucker);

		checkDeath();
	}

	public void takeDamageBullet(float damage, string animation, GameObject impact, Vector3 colliderHit){ // give another parameter, detect player from bullet skill script
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		//		if (playerControl.skill02Buffed)
		//		{
		//			currentHealth -= damage * 1.5f;
		//			CmdAnimation (animation);
		//		}
		//		else
		//		{
		HealthManager.singleton.takeDamage (playerNumber, damage,playerControl); //(damage * playerSkillControl.fillCharge.fillAmount);
		CmdAnimation (animation);
		//		}
		//CmdHit ();

		impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
		NetworkServer.Spawn (impactGO);
		Destroy (impactGO, 2.0f);

		checkDeath();
	}

	public void takeSkill02(float damage, string animation){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		HealthManager.singleton.takeDamage (playerNumber, damage,playerControl);
		CmdAnimation (animation);

		checkDeath();
	}

	public void takeDamageHazard(float damage){//, string animation){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		if (harzardDamageCD)
			return;
		HealthManager.singleton.takeDamage (playerNumber, damage,playerControl);
		gameObject.GetComponent<Animator>().SetBool("LegPainBool", true);
		legPainAnimation("LegPain");
		//CmdAnimation (animation);
//		harzardDamageCD = true;
//		StartCoroutine(hazardCoolDown());
		//CmdHit ();
		checkDeath();
	}

	public void legPainAnimation(string animation)
	{
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		if (harzardDamageCD)
			return;

		CmdAnimation (animation);
	}

	public void takeKnockbackDamage(float damage, string animation, GameObject impact, Vector3 position, Vector3 euler,Vector3 colliderHit,Collider other, GameObject guardImpact){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
		if (playerControl.state == PlayerControl.playerState.Guarding) {
			HealthManager.singleton.takeDamage (playerNumber, 1.0f,playerControl);
			playerControl.CmdPlaySFXClip(6);

			CmdGuardParticle();
			//m_SuperGuard.Play();
			//impactGO =  (GameObject)Instantiate (superGuard, position, Quaternion.identity);
			//NetworkServer.Spawn (impactGO);
			//Destroy (impactGO, 1.5f);
		}
		else {
			HealthManager.singleton.takeDamage (playerNumber, damage,playerControl);
			CmdAnimation (animation);

			impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
			NetworkServer.Spawn (impactGO);
			Destroy (impactGO, 1f);
		}

//		transform.root.LookAt (position);
//		Vector3 eulerFucker = euler;
//		eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
//		transform.root.rotation = Quaternion.Euler (eulerFucker);

		checkDeath();
	}

	public void takeMuaiThaiUlt(float damage, string animation, GameObject impact, Vector3 position, Vector3 euler,Vector3 colliderHit,Collider other){
		if (!isServer)
			return;
		if (playerControl.state == PlayerControl.playerState.Death || playerControl.invincible) {
			return;
		}
			HealthManager.singleton.takeDamage (playerNumber, damage,playerControl);
			CmdAnimation (animation);
		//}
		//CmdHit ();

		// only server side do, so the kena hit look won't trigger in client
		impactGO =  (GameObject)Instantiate (impact,colliderHit, Quaternion.identity);
		NetworkServer.Spawn (impactGO);
		Destroy (impactGO, 1.5f);

//		transform.root.LookAt (position);
//		Vector3 eulerFucker = euler;
//		eulerFucker = new Vector3 (0, eulerFucker.y - 180f, 0);
//		transform.root.rotation = Quaternion.Euler (eulerFucker);

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
			//CmdLose ();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		
		if (other.gameObject.CompareTag("OutOfBoundDeathZone"))
		{
			HealthManager.singleton.takeDamage (playerNumber, 200, playerControl);
			playerControl.CmdPlaySFXClip(7);
		}
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
	void CmdGuardParticle(){
		RpcGuardParticle();
	}

	[ClientRpc]
	void RpcGuardParticle(){
		m_SuperGuard.Play();
	}

	[Command]
	void CmdFallToDeath(){
		HealthManager.singleton.takeDamage (playerNumber, 200.0f,playerControl);
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
