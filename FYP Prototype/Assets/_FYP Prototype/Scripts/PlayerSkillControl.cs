using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerSkillControl : PlayerControl
{

	[Header("Skill01")]
	public GameObject projectile;
	public GameObject projectileMax;
	//public Transform playerParented;
	public Transform skill01SpawnPoint;

	public Image fillCharge;
	public GameObject chargeBar;

	public bool maxCharge;
	public float chargeRate;


	[Header("Skill02")]
	public bool skill02Buffed;
	public ParticleSystem skill02ParticleEffecct;


	[Header("Ultimate")]
	public GameObject ultiAttack;


	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		BarScan();
		InputSkills();
		SkillCooldown();
	}

	void BarScan()
	{
		if (chargeBar.activeInHierarchy)
		{
			state = playerState.SkillCharging;
		}
	}

	public void InputSkills()
	{
		if (state == playerState.Normal)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (skill01Cooldown <= 0)
				{
					maxCharge = false;
					chargeBar.SetActive(true);
					CmdAnimation("Bullet");
					animation.SetBool("ReleaseShot", false);
					skill01CD.fillAmount = 1;
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				if (skill02Cooldown <= 0)
				{
					CmdShowParticle();
					CmdAnimation("Wall");

					skill02CD.fillAmount = 1;
					skill02Cooldown = skill02CooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");

					ultimateCD.fillAmount = 1;
					ultimateCooldown = ultimateCooldownDuration;
				}
			}
		}
		else if (state == playerState.SkillCharging)
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				if (skill01Cooldown <= 0 && !maxCharge)
				{
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					//CmdFire (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					CmdFire (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					StartCoroutine(ResetDelay());
					skill01Cooldown = skill01CooldownDuration;
				}
				else if (skill01Cooldown <= 0 && maxCharge)
				{
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					//CmdFire02 (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					CmdFire02 (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					StartCoroutine(ResetDelay());
					skill01Cooldown = skill01CooldownDuration;
				}
			}
		}

		if (chargeBar.activeInHierarchy)
		{
			fillCharge.fillAmount += Time.deltaTime * chargeRate;

			if (fillCharge.fillAmount >= 1)
			{
				maxCharge = true;
			}
		}
	}

	IEnumerator ResetDelay()
	{
		yield return new WaitForSeconds(skill01CooldownDuration - 1);
		fillCharge.fillAmount = 0;
	}

	void UltiAcitve()
	{
		ultiAttack.SetActive(true);
	}

	void UltiNotAcitve()
	{
		ultiAttack.SetActive(false);
	}

	[Command]
	public void CmdFire(Vector3 position, Quaternion rotation, bool max){
		var bullet = Instantiate(projectile, position, rotation);
		bullet.GetComponent<BulletSkill> ().setMaxCharge (max);
		NetworkServer.Spawn (bullet);
	}

	[Command]
	public void CmdFire02(Vector3 position, Quaternion rotation, bool max){
		var bullet = Instantiate(projectileMax, position, rotation);
		bullet.GetComponent<BulletSkill> ().setMaxCharge (max);
		NetworkServer.Spawn (bullet);
	}

	[Command] // call by client to request the server to do this function (damage)
	void CmdShowParticle()
	{
		RpcShowParticle();
		skill02Buffed = true;
	}

	[ClientRpc]
	void RpcShowParticle() // show both side
	{
		skill02ParticleEffecct.Play();
	}
}