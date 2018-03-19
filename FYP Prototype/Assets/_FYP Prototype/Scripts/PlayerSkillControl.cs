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
	public ParticleSystem skill01ParticleEffect;
	public ParticleSystem _skill01ParticleEffect;

	public Image fillCharge;
	public GameObject chargeBar;

	public bool maxCharge;
	public float chargeRate;


	[Header("Skill02")]
	public bool skill02Buffed;
	public ParticleSystem skill02ParticleEffect;


	[Header("Ultimate")]
	public GameObject ultiAttack;
	public ParticleSystem ultiParticleEffect;

	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		BarScan();
		InputSkills();
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
			if (KeyBindingManager.GetKeyDown(KeyAction.Skill01))
			{
				if (skill01Cooldown <= 0)
				{
					maxCharge = false;
					chargeBar.SetActive(true);
					CmdSkill01PlayParticle();
					CmdAnimation("Bullet");
					animation.SetBool("ReleaseShot", false);
					skill01CD.fillAmount = 1;
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Skill02))
			{
				if (skill02Cooldown <= 0)
				{
					CmdSkill02PlayParticle();
					CmdAnimation("Wall");

					skill02CD.fillAmount = 1;
					skill02Cooldown = skill02CooldownDuration;

					soundEffect.PlaySFX(SFXAudioClipID.SFX_SKILL02);
				}
			}
			else if (KeyBindingManager.GetKeyDown(KeyAction.Ultimate))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");
					CmdUltiPlayParticle();

					ultimateCD.fillAmount = 1;
					ultimateCooldown = ultimateCooldownDuration;
				}
			}
		}
		else if (state == playerState.SkillCharging)
		{
			if (KeyBindingManager.GetKeyUp(KeyAction.Skill01))
			{
				if (skill01Cooldown <= 0 && !maxCharge)
				{
					CmdSkill01StopParticle();
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					CmdFire (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					StartCoroutine(ResetDelay());
					skill01Cooldown = skill01CooldownDuration;

					soundEffect.PlaySFX(SFXAudioClipID.SFX_SKILL01);
				}
				else if (skill01Cooldown <= 0 && maxCharge)
				{
					CmdSkill01StopParticle();
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					CmdFire02 (skill01SpawnPoint.position, skill01SpawnPoint.rotation, maxCharge);
					chargeBar.SetActive(false);

					StartCoroutine(ResetDelay());
					skill01Cooldown = skill01CooldownDuration;

					soundEffect.PlaySFX(SFXAudioClipID.SFX_SKILL01);
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

	void Skill01Casting()
	{
		CmdSkill01PlayParticle();
	}

	void Skill01NotCasting()
	{
		CmdSkill01StopParticle();
	}

	void UltiAcitve()
	{
		ultiAttack.SetActive(true);
		soundEffect.PlaySFX(SFXAudioClipID.SFX_ULTIMATE);
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

	[Command]
	void CmdSkill02PlayParticle()
	{
		RpcSkill02PlayParticle();
		skill02Buffed = true;
	}

	[ClientRpc]
	void RpcSkill02PlayParticle()
	{
		skill02ParticleEffect.Play();
	}

	[Command]
	void CmdSkill01PlayParticle()
	{
		RpcSkill01PlayParticle();
	}

	[ClientRpc]
	void RpcSkill01PlayParticle()
	{
		_skill01ParticleEffect.Play();
		skill01ParticleEffect.Play();
	}

	[Command]
	void CmdSkill01StopParticle()
	{
		RpcSkill01StopParticle();
	}

	[ClientRpc]
	void RpcSkill01StopParticle()
	{
		_skill01ParticleEffect.Stop();
		skill01ParticleEffect.Stop();
	}

	[Command]
	void CmdUltiPlayParticle()
	{
		RpcUltiPlayParticle();
	}

	[ClientRpc]
	void RpcUltiPlayParticle()
	{
		ultiParticleEffect.Play();
	}
}