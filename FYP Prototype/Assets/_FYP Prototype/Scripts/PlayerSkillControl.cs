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
	public Transform skill01SpawnPoint;

	public Image skill01CD;
	public Image fillCharge;
	public GameObject chargeBar;

	public bool maxCharge;
	public float chargeRate;

	float skill01Cooldown;
	public float skill01CooldownDuration;


	[Header("Skill02")]
	public Image skill02CD;
	public bool skill02Buffed;

	float skill02Cooldown;
	public float skill02CooldownDuration;

	public ParticleSystem skill02ParticleEffecct;


	[Header("Ultimate")]
	public Image ultiCD;
	public GameObject ultiAttack;

	float ultiCooldown;
	public float ultiCooldownDuration;


	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		BarScan();
		InputSkills();
	}

//	void Update()
//	{
//		if (!isLocalPlayer)
//			return;
//		BarScan();
//		InputSkills();
//	}

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
				if (ultiCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");

					ultiCD.fillAmount = 1;
					ultiCooldown = ultiCooldownDuration;
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
					CmdFire (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					skill01Cooldown = skill01CooldownDuration;
				}
				else if (skill01Cooldown <= 0 && maxCharge)
				{
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					CmdFire02 (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					skill01Cooldown = skill01CooldownDuration;
				}
			}
		}

		if (skill01Cooldown > 0)
		{
			skill01CD.fillAmount -= 1.0f / skill01CooldownDuration * Time.deltaTime;
			skill01Cooldown -= Time.deltaTime;
		}
		else
		{
			skill01Cooldown = 0;
		}

		if (chargeBar.activeInHierarchy)
		{
			fillCharge.fillAmount += Time.deltaTime * chargeRate;

			if (fillCharge.fillAmount >= 1)
			{
				maxCharge = true;
			}
		}
		else
		{
			fillCharge.fillAmount = 0;
		}

		if (skill02Cooldown > 0)
		{
			skill02CD.fillAmount -= 1.0f / skill02CooldownDuration * Time.deltaTime;
			skill02Cooldown -= Time.deltaTime;
		}
		else
		{
			skill02Cooldown = 0;
		}

		if (ultiCooldown > 0)
		{
			ultiCD.fillAmount -= 1.0f / ultiCooldownDuration * Time.deltaTime;
			ultiCooldown -= Time.deltaTime;
		}
		else
		{
			ultiCooldown = 0;
		}
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