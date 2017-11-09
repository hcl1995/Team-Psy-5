using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl03 : MonoBehaviour
{
	private static PlayerControl03 _instance;
	public static PlayerControl03 Instance
	{
		get 
		{
			if(_instance == null)
			{
				GameObject go = GameObject.Find("Player");

				_instance = go.GetComponent<PlayerControl03>();
				_instance.Start();
			}
			return _instance;
		}
	}

	float rotationSpeed = 30;

	Vector3 movement;
	Vector3 targetDirection;

	Rigidbody rb;
	Animator animation;

	[Header("Movement")]
	public float speed;
	public float dashing;
	public float dashChargeCooldownDuration;
	public int maxDashCharge;

	Vector3 startPos;
	Vector3 endPos;

	int count;
	bool dash = false;
	float completeTime;
	float lerpSpeed = 10;

	[Header("Bullet")]
	public float chargeRate;
	public float bulletCooldownDuration;

	[Header("EarthWallJutsu")]
	public float wallCooldownDuration;

	[Header("Ultimate")]
	public float ultimateCooldownDuration;

	[Header("Drag & Drop")]
	public GameObject projectile;
	public Transform spawnPoint;
	public GameObject chargeBar;
	public Image fillCharge;
	public GameObject wallSkill;
	public GameObject wallSkill02;
	public Transform wallSpawnPoint;
	public Transform wallSpawnPoint02;
	public Transform wallSpawnPoint03;
	public Transform wallSpawnPoint04;
	public GameObject ultimate;
	public Transform ultimateSpawnPoint;
	public GameObject trailRendererObject;

	[Header("Observer Ward")]
	public bool maxCharge;
	public int dashCharge;
	public float wallCooldown;
	public float bulletCooldown;
	public float ultimateCooldown;
	public float dashChargeCooldown;

	public int attack;
	public float attackInterval;
	public float attackIntervalLimit;

	[HideInInspector]
	public Transform recordEndPos;

	[Header("Hitboxes")]
	public GameObject attack01;
	public GameObject attack02;
	public GameObject attack03;

	public enum playerState
	{
		Normal,
		Guarding, // def++?
		Attacking,
		SkillCharging,
		OnAnimation
	}

	public playerState state = playerState.Normal;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
	}

	void Start()
	{
		dashCharge = maxDashCharge;
	}

	void Update()
	{
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

		Attack();
		//Movement(x, z);
		ActionsInputKey();
	}

	void Movement(float x, float z)
	{
		movement.Set (x, 0, z);
		movement = movement.normalized * speed * Time.deltaTime;
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
					z = dashing;
					startPos = transform.position;
					endPos = transform.position += (transform.forward * z);

					dash = true;
					animation.SetTrigger("Dash");

					count++;
					dashCharge--;
					dashChargeCooldown += dashChargeCooldownDuration;
				}
			}
		}

		if (dash)
		{
			completeTime += (Time.deltaTime * lerpSpeed);
			transform.position = Vector3.Lerp (startPos, endPos, completeTime);
			trailRendererObject.transform.position = gameObject.transform.position;
		}

		if (completeTime >= 1)
		{
			dash = false;
			completeTime = 0;
		}
			
		if (dashChargeCooldown > 0)
		{
			dashChargeCooldown -= Time.deltaTime;
		}
		else
		{
			dashChargeCooldown = 0;
		}

		// GG gives me cancer
		if (maxDashCharge - dashCharge == count && dashChargeCooldown <= ((dashChargeCooldownDuration * count) - dashChargeCooldownDuration))
		{
			count--;
			dashCharge++;
		}

		// hardcoded..
//		if (maxDashCharge - dashCharge == 1 && dashChargeCooldown <= 0)
//		{
//			dashCharge++;
//		}
//		else if (maxDashCharge - dashCharge == 2 && dashChargeCooldown <= 5)
//		{
//			dashCharge++;
//		}
//		else if (maxDashCharge - dashCharge == 3 && dashChargeCooldown <= 10)
//		{
//			dashCharge++;
//		}
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

	void RotateTowardMouseDuringAction()
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

	void ActionsInputKey()
	{
//		if (Input.GetMouseButtonDown(0))
//		{
//			RotateTowardMouseDuringAction();
//			animation.SetTrigger("Attack");
//		}
		if (state == playerState.SkillCharging)
		{
			if (Input.GetKeyUp(KeyCode.Q))
			{
				if (bulletCooldown <= 0)
				{
					animation.SetBool("ReleaseShot", true);

					RotateTowardMouseDuringAction();
					Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
					chargeBar.SetActive(false);

					bulletCooldown = bulletCooldownDuration;
				}
			}
		}
		else if (state == playerState.Normal)
		{
			if (Input.GetMouseButtonDown(1))
			{
				RotateTowardMouseDuringAction();
				animation.SetTrigger("Guard");
				animation.SetBool("Guarding", true);
			}
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				if (bulletCooldown <= 0)
				{
					maxCharge = false;
					chargeBar.SetActive(true);
					animation.SetTrigger("Bullet");
					animation.SetBool("ReleaseShot", false);
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				if (wallCooldown <= 0)
				{
					recordEndPos = OpponentChan.Instance.transform;

					animation.SetTrigger("Wall");
					Instantiate(wallSkill, wallSpawnPoint.position, wallSpawnPoint.rotation);
					Instantiate(wallSkill, wallSpawnPoint02.position, wallSpawnPoint02.rotation);
					Instantiate(wallSkill, wallSpawnPoint03.position, wallSpawnPoint03.rotation);
					Instantiate(wallSkill02, wallSpawnPoint04.position, wallSpawnPoint04.rotation);

					wallCooldown = wallCooldownDuration;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (ultimateCooldown <= 0)
				{
					RotateTowardMouseDuringAction();
					//animation.SetTrigger("Ultimate");
					Instantiate(ultimate, ultimateSpawnPoint.position, ultimateSpawnPoint.rotation);

					ultimateCooldown = ultimateCooldownDuration;
				}
			}
		}
		else if (state == playerState.Guarding)
		{
			if (Input.GetMouseButtonUp(1))
			{
				animation.SetBool("Guarding", false);
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

	void Attack()
	{
		if (attack >= 3 || attackInterval > attackIntervalLimit)
		{
			attack = 0;
			attackInterval = 0;
		}
		else if (attack >= 1 && attackInterval < attackIntervalLimit)
		{
			attackInterval += Time.deltaTime;
		}
			
		if (Input.GetMouseButtonDown(0))
		{
			attack++;
			attackInterval = 0;
			//Debug.Log(attack);
		}

		if (state == playerState.Normal)
		{
			if (attack == 1 && attackInterval < attackIntervalLimit)
			{
				RotateTowardMouseDuringAction();
				animation.SetTrigger("Attack");
			}
		}
		else if (state == playerState.Attacking)
		{
			if (attack == 2 && attackInterval < attackIntervalLimit)
			{
				RotateTowardMouseDuringAction();
				animation.SetTrigger("Attack02");
			}
			else if (attack >= 3 && attackInterval < attackIntervalLimit)
			{
				attack = 0;
				attackInterval = 0;

				RotateTowardMouseDuringAction();
				animation.SetTrigger("Attack03");
			}
		}
			
		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			attack01.SetActive(true);
		}
		else
		{
			attack01.SetActive(false);
		}

		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
		{
			attack02.SetActive(true);
		}
		else
		{
			attack02.SetActive(false);
		}

		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
		{
			attack03.SetActive(true);
		}
		else
		{
			attack03.SetActive(false);
		}
	}

	void RestrictInput()
	{
		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Run") ||
			this.animation.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
		{
			state = playerState.Normal;
			animation.ResetTrigger("Attack");
			animation.ResetTrigger("Attack02");
			animation.ResetTrigger("Attack03");
		}
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
		{
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
		else if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("ShootCasting02") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Wall") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("Ultimate") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown02") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("DamageDown03") ||
				 this.animation.GetCurrentAnimatorStateInfo(0).IsName("Recover") || this.animation.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			state = playerState.OnAnimation;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("OutofBound"))
		{
			rb.drag = 1;
			rb.constraints = RigidbodyConstraints.None;
		}
		else
		{
			rb.drag = Mathf.Infinity;
			rb.constraints = RigidbodyConstraints.FreezePositionY |	RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
	}
}