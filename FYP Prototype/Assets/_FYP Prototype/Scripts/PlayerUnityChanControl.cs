using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnityChanControl : PlayerControl03
{
	[SerializeField]
	GameObject charSkill02;
	[SerializeField]
	Transform skill02SpawnPoint;

	protected override void ActionsInputKey()
	{
		if (state == playerState.Normal)
		{
			if (Input.GetMouseButtonDown(1))
			{
				RotateTowardMouseDuringAction();
				animation.SetTrigger("Guard");
				animation.SetBool("Guarding", true);
				CmdSetPlayerState (playerState.Guarding);
			}
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				if (bulletCooldown <= 0)
				{
					animation.SetTrigger("Bullet");
					RotateTowardMouseDuringAction();
					CmdFire (spawnPoint.position,spawnPoint.rotation);

					bulletCooldown = bulletCooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				if (wallCooldown <= 0)
				{
					animation.SetTrigger("Wall");
					charSkill02.SetActive(true);
					//CmdSkill02 (skill02SpawnPoint.position, skill02SpawnPoint.rotation);

					wallCooldown = wallCooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
				}
			}
		}
		else if (state == playerState.Guarding)
		{
			if (Input.GetMouseButtonUp(1))
			{
				animation.SetBool("Guarding", false);
				CmdSetPlayerState (playerState.Normal);
			}
		}

		if (bulletCooldown > 0)
		{
			bulletCooldown -= Time.deltaTime;
		}
		else
		{
			bulletCooldown = 0;
		}

		if (wallCooldown > 0)
		{
			wallCooldown -= Time.deltaTime;
		}
		else
		{
			wallCooldown = 0;
		}

		if (ultimateCooldown > 0)
		{
			ultimateCooldown -= Time.deltaTime;
		}
		else
		{
			ultimateCooldown = 0;
		}
	}

	[Command]
	void CmdSkill02(Vector3 position, Quaternion rotation){
		var skill02 = Instantiate(charSkill02, position, rotation);
		NetworkServer.Spawn (skill02);
	}
}