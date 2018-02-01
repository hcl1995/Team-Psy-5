using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	Vector3 movement;
	Vector3 targetDirection;
	float rotationSpeed = 30;

	Rigidbody rb;
	public Animator animation;


	[Header("Movement")]
	public float moveSpeed;
	public float dashDistance;

	int dashCount;
	int dashCharge;
	float dashChargeCooldown;
	public int maxDashChargeCount;
	public float dashChargeCooldownDuration;

	Vector3 endPos;
	Vector3 startPos;
	bool isDash = false;
	float completeDashTime;
	float dashLerpSpeed = 10;


	[Header("Attack")]
	public GameObject attack01;
	public GameObject attack02;
	public GameObject attack03;

	int attackCount;
	float attackInterval;
	public float attackIntervalLimit;


//	[Header("Skill Cooldown")]
//	float skill01Cooldown;
//	public float skill01CooldownDuration;
//	public Image skill01CD;
//
//	float skill02Cooldown;
//	public float skill02CooldownDuration;
//	public Image skill02CD;
//
//	float ultimateCooldown;
//	public float ultimateCooldownDuration;
//	public Image ultimateCD;


	[Header("Drag & Drop")]
	public GameObject playerCanvas;
	public GameObject trailRendererObject;
	public GameObject particleGuard;


	public enum playerState
	{
		Normal,
		Guarding,
		Attacking,
		SkillCharging,
		OnAnimation
	}

	public playerState state = playerState.Normal;

	bool callOnce;
	bool toggleGuard = false;


	protected void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
		trailRendererObject.transform.parent = null;
	}

	protected void Start()
	{
		dashCharge = maxDashChargeCount;
		if (isLocalPlayer){
			playerCanvas.SetActive (true);
		}
	}

//	protected void Update()
//	{
//		if (!isLocalPlayer)
//			return;
//		CheckInput();
//	}

	protected void CheckInput(){
		RestrictInput();

		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		movement = new Vector3(x, 0, z);

		if (state == playerState.Normal)
		{
			Movement(x, z);
		}
		else
		{
			Movement(0, 0);
		}

		Guard();
		Attack();

		if (Input.GetKeyDown (KeyCode.K)) {
			if (toggleGuard) {
				animation.SetBool ("Guarding", false);
				CmdSetPlayerState (playerState.Normal);
				toggleGuard = false;
			} else if (!toggleGuard) {
				animation.SetTrigger("Guard");
				animation.SetBool("Guarding", true);
				CmdSetPlayerState (playerState.Guarding);
				toggleGuard = true;
			}
		}
	}

	void Movement(float x, float z)
	{
		movement.Set (x, 0, z);
		movement = movement.normalized * moveSpeed * Time.deltaTime;

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.MovePosition (transform.position + movement);

		bool moving = x != 0f || z != 0f;
		animation.SetBool("isMoving", moving);

		GetCameraRelativeMovement();
		RotateTowardMovementDirection();

		if (state == playerState.Normal)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				if (dashCharge > 0)
				{
					z = dashDistance;
					startPos = transform.position;
					endPos = transform.position += (transform.forward * z);

					isDash = true;
					CmdAnimation("Dash");

					dashCount++;
					dashCharge--;
					dashChargeCooldown += dashChargeCooldownDuration;
				}
			}
		}

		if (isDash)
		{
			completeDashTime += (Time.deltaTime * dashLerpSpeed);
			transform.position = Vector3.Lerp (startPos, endPos, completeDashTime);
			//trailRendererObject.transform.position = gameObject.transform.position;
			CmdDashTrail();
		}

		if (completeDashTime >= 1)
		{
			isDash = false;
			completeDashTime = 0;
		}
			
		if (dashChargeCooldown > 0)
		{
			dashChargeCooldown -= Time.deltaTime;
		}
		else
		{
			dashChargeCooldown = 0;
		}
			
		if (maxDashChargeCount - dashCharge == dashCount && dashChargeCooldown <= ((dashChargeCooldownDuration * dashCount) - dashChargeCooldownDuration))
		{
			dashCount--;
			dashCharge++;
		}
	}

	//converts control input vectors into camera facing vectors
	void GetCameraRelativeMovement()
	{  
		Transform cameraTransform = Camera.main.transform;

		// Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;

		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		//directional inputs
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		// Target direction relative to the camera
		targetDirection = x * right + z * forward;
	}

	void RotateTowardMovementDirection()  
	{
		if (movement != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
		}
	}

	public void RotateTowardMouseDuringAction()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit))
		{
			transform.LookAt(hit.point);

			// bloody cheat
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
//			Quaternion rotation = Quaternion.LookRotation(hit.point);
//			transform.rotation = rotation;

//			Vector3 playerToMouse = hit.point - transform.position;
//
//			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
//
//			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
//
//			rb.MoveRotation (newRotation);
		}
	}

	void Guard()
	{
		if (state == PlayerControl.playerState.Normal)
		{
			if (Input.GetMouseButtonDown(1))
			{
				RotateTowardMouseDuringAction();
				animation.SetTrigger("Guard");
				animation.SetBool("Guarding", true);
				CmdSetPlayerState (PlayerControl.playerState.Guarding);
			}
		}
		else if (state == PlayerControl.playerState.Guarding)
		{
			if (Input.GetMouseButtonUp(1))
			{
				animation.SetBool("Guarding", false);
				CmdSetPlayerState (PlayerControl.playerState.Normal);
			}
		}
	}

	void Attack()
	{
		if (attackCount >= 3 || attackInterval > attackIntervalLimit)
		{
			attackCount = 0;
			attackInterval = 0;
		}
		else if (attackCount >= 1 && attackInterval < attackIntervalLimit)
		{
			attackInterval += Time.deltaTime;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Confined;
			attackCount++;
			attackInterval = 0;
		}

		if (state == playerState.Normal)
		{
			if (attackCount == 1 && attackInterval < attackIntervalLimit)
			{
				RotateTowardMouseDuringAction();
				CmdAnimation("Attack");
			}
		}
		else if (state == playerState.Attacking)
		{
			if (attackCount == 2 && attackInterval < attackIntervalLimit)
			{
				RotateTowardMouseDuringAction();
				CmdAnimation("Attack02");
			}
			else if (attackCount >= 3 && attackInterval < attackIntervalLimit)
			{
				attackCount = 0;
				attackInterval = 0;

				RotateTowardMouseDuringAction();
				CmdAnimation("Attack03");
			}
		}
	}

	void Atk01Active()
	{
		attack01.SetActive(true);
		//animation.ResetTrigger("Attack");
	}

	void Atk01NotActive()
	{
		attack01.SetActive(false);
	}

	void Atk02Active()
	{
		attack02.SetActive(true);
	}

	void Atk02NotActive()
	{
		attack02.SetActive(false);
	}

	void Atk03Active()
	{
		attack03.SetActive(true);
		//animation.ResetTrigger("Attack02");
		//animation.ResetTrigger("Attack03");
	}

	void Atk03NotActive()
	{
		attack03.SetActive(false);
	}

	void RestrictInput()
	{
		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Run") ||
			this.animation.GetCurrentAnimatorStateInfo(0).IsName("Dash") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("ShootCasting02"))
		{
			state = playerState.Normal;
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
		{
			callOnce = false;
			state = playerState.Attacking;
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
		{
			state = playerState.Guarding;
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("ShootCasting01"))
		{
			state = playerState.SkillCharging;
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Wall") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Ultimate") || 
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown02") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown03") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("Recover") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			state = playerState.OnAnimation;
		}
		CmdSetActive();
	}

	[Command]
	void CmdSetActive()
	{
		RpcSetActive();
	}

	[ClientRpc]
	void RpcSetActive()
	{
		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Run") ||
			this.animation.GetCurrentAnimatorStateInfo(0).IsName("Dash") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("ShootCasting02"))
		{
			particleGuard.SetActive(false);
			if (callOnce == false)
			{
				animation.ResetTrigger("Attack01");
				animation.ResetTrigger("Attack02");
				animation.ResetTrigger("Attack03");
				callOnce = true;
			}
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
		{
			particleGuard.SetActive(true);
		}
	}
		
	[Command]
	public void CmdAnimation(string anim){
		RpcSetAnimation (anim);
	}

	[ClientRpc]
	void RpcSetAnimation(string anim){
		animation.SetTrigger(anim);

//		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
//		{
//			attack01.SetActive(true);
//			animation.ResetTrigger("Attack");
//		}
//		else
//		{
//			attack01.SetActive(false);
//		}
//
//		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
//		{
//			attack02.SetActive(true);
//		}
//		else
//		{
//			attack02.SetActive(false);
//		}
//
//		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
//		{
//			attack03.SetActive(true);
//			animation.ResetTrigger("Attack02");
//			animation.ResetTrigger("Attack03");
//		}
//		else
//		{
//			attack03.SetActive(false);
//		}
	}

	[Command]
	public void CmdSetPlayerState(playerState playerState){
		state = playerState;
	}

	[Command]
	public void CmdDashTrail(){
		RpcDashTrail();
	}

	[ClientRpc]
	public void RpcDashTrail(){
		trailRendererObject.transform.position = gameObject.transform.position;
	}

	protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("OutofBound"))
		{
			rb.drag = 1;
			rb.constraints = RigidbodyConstraints.None;
			animation.SetBool("OnGround", false);
		}
		//		else
		//		{
		//			rb.drag = Mathf.Infinity;
		//			rb.constraints = RigidbodyConstraints.FreezePositionY |	RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		//			animation.SetBool("OnGround", true);
		//		}
	}
}