using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
	Vector3 targetDirection;
	float rotationSpeed = 30;

	public Animator animation;
	public Vector3 startSpawnPosition;

	[Header("Movement")]
	public float speed;
	public float gravity;
	public Vector3 moveDirection = Vector3.zero;
	public float dashDistance;
	//Vector3 moveDirectionX = Vector3.right;
	//Vector3 moveDirection_X = Vector3.left;
	//Vector3 moveDirectionZ = Vector3.forward;
	//Vector3 moveDirection_Z = Vector3.back;


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


	[Header("Skill Cooldown w Icon")]
	public float skill01CooldownDuration;
	[HideInInspector]
	public float skill01Cooldown;
	public Image skill01CD;

	[HideInInspector]
	public float skill02Cooldown;
	public float skill02CooldownDuration;
	public Image skill02CD;

	[HideInInspector]
	public float ultimateCooldown;
	public float ultimateCooldownDuration;
	public Image ultimateCD;


	[Header("Drag & Drop")]
	public GameObject playerCanvas;
	public GameObject trailRendererObject;
	public GameObject particleGuard;
	public GameObject CharaterModel;

	[Header("Observer")]
	public bool isFalling = false;

	public List <GameObject> inBetweenObjects = new List<GameObject>();
	public float materialAlpha;
	GameObject damnCamera;

	[SyncVar]
	public bool invincible = false;
	public float blinkTime = 0.3f;
	public float invincibleDuration = 3.0f;
	public float invincibleElapsed = 0;

	public enum playerState
	{
		Normal,
		Guarding,
		Attacking,
		SkillCharging,
		OnAnimation,
		Death
	}

	[SyncVar]
	public playerState state = playerState.Normal;
	[SyncVar]
	bool callOnce;
	bool toggleGuard = false;


	protected void Awake()
	{
		animation = GetComponent<Animator>();
		trailRendererObject.transform.parent = null;
		damnCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}

	protected void Start()
	{
		dashCharge = maxDashChargeCount;
		if (isLocalPlayer){
			playerCanvas.SetActive (true);
		}
		startSpawnPosition = gameObject.transform.root.position;
	}

	protected void CheckInput(){

		RestrictInput();
		Guard();
		Attack();
		Movement();
		SkillCooldown();
		CmdTransparentObjects();

		if (invincible) {
			blinkTime += Time.deltaTime;
			invincibleElapsed += Time.deltaTime;
			if (invincibleElapsed <= invincibleDuration) {
				if (blinkTime >= 0.3) {
					if (CharaterModel.activeSelf)
						CmdBlinkCharacter (false);
					else
						CmdBlinkCharacter (true);
					blinkTime = 0.0f;
				}
			} else {
				CmdInvincible (false);
				CmdBlinkCharacter (true);
				invincibleElapsed = 0.0f;
				blinkTime = 0.3f;
			}
		} else {
			CmdBlinkCharacter (true);
		}
	}

	void Movement()
	{
		CharacterController controller = GetComponent<CharacterController>();

		controller.Move(Vector3.down * gravity * Time.deltaTime);

		if (isFalling)
		{
			// add fall animation --> OnAnim
			//state = playerState.OnAnimation;
			animation.SetBool("OnGround", false);
		}
		else
		{
			animation.SetBool("OnGround", true);
		}

		if (state == playerState.Normal)
		{
			//GetAxis will be smoothed.
			//moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			//moveDirection *= speed;
			//controller.Move(moveDirection * Time.deltaTime);

			moveDirection = new Vector3(0, 0, 0);

			// Fucking Game Input Manager..
			if (KeyBindingManager.GetKey(KeyAction.Up))
			{
				moveDirection = new Vector3(0, 0, 1);
				controller.Move(moveDirection * speed * Time.deltaTime);
			}
			else if (KeyBindingManager.GetKey(KeyAction.Down))
			{
				moveDirection = new Vector3(0, 0, -1);
				controller.Move(moveDirection * speed * Time.deltaTime);
			}

			if (KeyBindingManager.GetKey(KeyAction.Left))
			{
				moveDirection = new Vector3(-1, 0, 0);
				controller.Move(moveDirection * speed * Time.deltaTime);
			}
			else if (KeyBindingManager.GetKey(KeyAction.Right))
			{
				moveDirection = new Vector3(1, 0, 0);
				controller.Move(moveDirection * speed * Time.deltaTime);
			}
			bool moving = moveDirection != Vector3.zero;
			animation.SetBool("isMoving", moving);

			GetCameraRelativeMovement();

			if (KeyBindingManager.GetKeyDown(KeyAction.Dash))
			{
				if (dashCharge > 0)
				{
					startPos = transform.position;
					endPos = transform.position += (transform.forward * dashDistance);

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

		// Target direction relative to the camera
		targetDirection = moveDirection.x * right + moveDirection.z * forward;

		if (moveDirection != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
		}
	}

	protected void RotateTowardMouseDuringAction()
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
		}
	}

	void Guard()
	{
		
		if (KeyBindingManager.GetKey(KeyAction.Guard))
		{
			if (state == PlayerControl.playerState.Normal) {
				//animation.SetBool("isMoving", false);
				RotateTowardMouseDuringAction();
				//animation.SetTrigger("Guard");
				if(!toggleGuard)
					CmdAnimation ("Guard");
				toggleGuard = true;
				animation.SetBool("Guarding", true);
				CmdSetPlayerState (PlayerControl.playerState.Guarding);
			}				
		}else if (state == PlayerControl.playerState.Guarding){
//			if (KeyBindingManager.GetKeyUp(KeyAction.Guard))
//			{
			//animation.SetBool("isMoving", false);
			animation.SetBool("Guarding", false);
			CmdSetPlayerState (PlayerControl.playerState.Normal);
			toggleGuard = false;
		//}
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

		if (KeyBindingManager.GetKeyDown(KeyAction.Attack))
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

	protected void SkillCooldown()
	{
		if (skill01Cooldown > 0)
		{
			skill01CD.fillAmount -= 1.0f / skill01CooldownDuration * Time.deltaTime;
			skill01Cooldown -= Time.deltaTime;
		}
		else
		{
			skill01Cooldown = 0;
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

		if (ultimateCooldown > 0)
		{
			ultimateCD.fillAmount -= 1.0f / ultimateCooldownDuration * Time.deltaTime;
			ultimateCooldown -= Time.deltaTime;
		}
		else
		{
			ultimateCooldown = 0;
		}
	}

	void Atk01Active()
	{
		attack01.SetActive(true);
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
	}

	void Atk03NotActive()
	{
		attack03.SetActive(false);
	}

	void RestrictInput()
	{
		if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Idle") || this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Run") ||
		    this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Dash") || this.animation.GetCurrentAnimatorStateInfo (0).IsName ("ShootCasting02")) {
			state = playerState.Normal;
		} else if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack") || this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack02") ||
		           this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Attack03")) {
			callOnce = false;
			state = playerState.Attacking;
			animation.SetBool("isMoving", false);
		} else if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Guard")) {
			state = playerState.Guarding;
			animation.SetBool("isMoving", false);
		} else if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("ShootCasting01")) {
			state = playerState.SkillCharging;
			animation.SetBool("isMoving", false);
		} else if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Wall") || this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Ultimate") ||
		           this.animation.GetCurrentAnimatorStateInfo (0).IsName ("DamageDown") ||
		           this.animation.GetCurrentAnimatorStateInfo (0).IsName ("DamageDown02") || this.animation.GetCurrentAnimatorStateInfo (0).IsName ("DamageDown03") ||
		           this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Recover")) {
			state = playerState.OnAnimation;
			animation.SetBool("isMoving", false);
		} else if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {
			state = playerState.Death;
			animation.SetBool("isMoving", false);
		}
		CmdSetPlayerState (state);
		CmdSetActive();
	}
	public void playDeathAnim(){
		CmdAnimation ("Death");
	}

	[Command]
	void CmdSetActive()
	{
		callOnce = false;
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
				animation.ResetTrigger("Attack");
				animation.ResetTrigger("Attack02");
				animation.ResetTrigger("Attack03");
				callOnce = true;
			}
		}
		if (this.animation.GetCurrentAnimatorStateInfo (0).IsName ("Guard")) {
			particleGuard.SetActive (true);
		} else {
			particleGuard.SetActive(false);
		}
	}
		
	[Command]
	public void CmdAnimation(string anim){
		RpcSetAnimation (anim);
	}

	[ClientRpc]
	void RpcSetAnimation(string anim){
		animation.SetTrigger(anim);
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
		
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("OutofBound"))
		{
			isFalling = true;
		}
	}

	[Command]
	void CmdTransparentObjects()
	{
		RpcTransparentObjects();
	}

	[ClientRpc]
	void RpcTransparentObjects()
	{
		//Vector3 playerOffset = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		RaycastHit[] hits;
		// Max Distance = Distance Between Camera & Characters | LayerMask = Walls / ANY Props Above Ground.
		hits = Physics.RaycastAll(transform.position, damnCamera.transform.position - transform.position, Mathf.Infinity, 1 << 8);
		//hits = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0, 0), damnCamera.transform.position - transform.position, Quaternion.identity, Mathf.Infinity, 1 << 8);

//		foreach (RaycastHit hit in hits)
//		{
//			Renderer rend = hit.collider.GetComponent<Renderer>();
//
//			//rend.material.shader = Shader.Find("Transparent/Diffuse");
//			Color tempColor = rend.material.color;
//			tempColor.a = 0.1f;
//			rend.material.color = tempColor;
//		}

//------------------------------------------------------------------------------------------------------------------------------------------------------

		inBetweenObjects.Clear();
		foreach (RaycastHit hit in hits)
		{
			inBetweenObjects.Add(hit.collider.gameObject);
			Renderer rend = hit.collider.GetComponent<Renderer>();

			if(inBetweenObjects.Contains(hit.collider.gameObject))
			{ 
				Color tempColor = rend.material.color;
				tempColor.a = materialAlpha;
				rend.material.color = tempColor;
			}
		}
	}

//	void OnDrawGizmos()
//	{
//		Gizmos.color = Color.red;
//		Gizmos.DrawWireCube(transform.position, new Vector3(0.25f, 0, 0));
//	}

	public void respawnNow(){
		RpcRespwan ();
		CmdAnimation ("Guard");
		CmdInvincible(true);
	}

	[ClientRpc]
	public void RpcRespwan(){		
		transform.root.position = new Vector3(startSpawnPosition.x,startSpawnPosition.y,startSpawnPosition.z);
		transform.root.TransformPoint(new Vector3(startSpawnPosition.x,startSpawnPosition.y,startSpawnPosition.z));
		isFalling = false;
	}
	[Command]
	void CmdInvincible(bool invinc){
		invincible = invinc;

		RpcBlinkCharacter (true);
	}

	[Command]
	void CmdBlinkCharacter(bool b){
		RpcBlinkCharacter (b);
	}

	[ClientRpc]
	void RpcBlinkCharacter(bool b){
		CharaterModel.SetActive (b);
	}
}