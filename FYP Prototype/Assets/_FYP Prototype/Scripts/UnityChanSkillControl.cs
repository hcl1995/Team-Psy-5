using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UnityChanSkillControl : PlayerControl
{
	[Header("Skill01")]
	public GameObject projectile;
	public Transform skill01SpawnPoint;

	float skill01Cooldown;
	public float skill01CooldownDuration;


	[Header("Skill02")]
	public GameObject skill02Object;
	public Transform skill02SpawnPoint;

	float skill02Cooldown;
	public float skill02CooldownDuration;


	[Header("Ultimate")]
	//Vector3 endPos;
	//Vector3 startPos;

	//bool isUltimating;
	//float animationTime;
	//public float ultiLerpSpeed;
	//public float fixedDistance;

	public GameObject ultimateObject;

	float ultimateCooldown;
	public float ultimateCooldownDuration;

	public GameObject drillKickParticle;


	void Update()
	{
		if (!isLocalPlayer)
			return;
		CheckInput();
		InputSkills();
	}

//	void Update()
//	{
//		if (!isLocalPlayer)
//			return;
//		InputSkills();
//	}

	public void InputSkills()
	{
		if (state == playerState.Normal)
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (skill01Cooldown <= 0)
				{
					CmdAnimation("Bullet");
					RotateTowardMouseDuringAction();
					CmdSkill01 (skill01SpawnPoint.position, skill01SpawnPoint.rotation);

					skill01Cooldown = skill01CooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				if (skill02Cooldown <= 0)
				{
					CmdAnimation("Wall");

					skill02Cooldown = skill02CooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					CmdAnimation("Ultimate");

					//startPos = transform.position;
					//endPos = transform.position += transform.forward * fixedDistance;

					//isUltimating = true;
					//CmdUltimateActive();

					ultimateCooldown = ultimateCooldownDuration;
				}
			}
		}

		if (skill01Cooldown > 0)
		{
			skill01Cooldown -= Time.deltaTime;
		}
		else
		{
			skill01Cooldown = 0;
		}

		if (skill02Cooldown > 0)
		{
			skill02Cooldown -= Time.deltaTime;
		}
		else
		{
			skill02Cooldown = 0;
		}

		if (ultimateCooldown > 0)
		{
			ultimateCooldown -= Time.deltaTime;
		}
		else
		{
			ultimateCooldown = 0;
		}

//		if (isUltimating)
//		{
//			animationTime += (Time.deltaTime * ultiLerpSpeed);
//			transform.position = Vector3.Lerp (startPos, endPos, animationTime);
//
//			CmdUltimateActive();
//		}
//
//		if (animationTime >= 1)
//		{
//			isUltimating = false;
//			animationTime = 0;
//		}
	}
		
	void Skill02Active()
	{
		skill02Object.SetActive(true);
	}

	void UltimateActive()
	{
		ultimateObject.SetActive(true);
		drillKickParticle.SetActive(true);
	}

	void UltimateNotActive()
	{
		ultimateObject.SetActive(false);
		drillKickParticle.SetActive(false);
	}

	void UltimateMoving()
	{
		transform.position += transform.forward * 3;
	}

	[Command]
	public void CmdSkill01(Vector3 position, Quaternion rotation){
		var skill01 = Instantiate(projectile, position, rotation);
		NetworkServer.Spawn (skill01);
	}
}