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
				GameObject go = GameObject.Find("unitychan");

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

	public float speed;
	public float dashing;
	float completeTime;

	//public Transform opponentChan;

	public GameObject projectile;
	public Transform spawnPoint;

	public GameObject chargeBar;
	public Image fillCharge;
	public float chargeRate;

	//float bulletCooldown;
	public float bulletCooldownDuration;

	[HideInInspector]
	public bool maxCharge;

	public GameObject wallSkill;
	public GameObject wallSkill02;
	public Transform wallSpawnPoint;
	public Transform wallSpawnPoint02;
	public Transform wallSpawnPoint03;
	public Transform wallSpawnPoint04;

	//float wallCooldown;
	public float wallCooldownDuration;

	public GameObject ultimate;
	public Transform ultimateSpawnPoint;

	//float ultimateCooldown;
	public float ultimateCooldownDuration;

	[Header("Cooldown Timer")]
	public float bulletCooldown;
	public float wallCooldown;
	public float ultimateCooldown;

	[HideInInspector]
	public Transform recordEndPos;

	Vector3 startPos;
	Vector3 endPos;

	bool dash = false;
	float lerpSpeed = 10;

	[HideInInspector]
	public GameObject trailRendererObject;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
	}

	void Start()
	{

	}

	void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		movement = new Vector3(x, 0, z);

		Cooldowns();
		Movement(x, z);
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

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			z = dashing;
			startPos = transform.position;
			endPos = transform.position += (transform.forward * z);

			dash = true;
			trailRendererObject.transform.position = gameObject.transform.position;

			animation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
			// Animation Problem: Re-dash Instead Go Back To Idle
		}

		if (dash)
		{
			completeTime += (Time.deltaTime * lerpSpeed);
			transform.position = Vector3.Lerp (startPos, endPos, completeTime);
		}

		if (completeTime >= 1)
		{
			dash = false;
			completeTime = 0;
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

	void RotateTowardOpponentDuringAction()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f, LayerMask.GetMask("Ground")))
		{
			transform.LookAt(hit.point);
		}
	}

	void ActionsInputKey()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RotateTowardOpponentDuringAction();
			animation.SetTrigger("Attack(LeftClick)");
		}
		else if (Input.GetMouseButtonDown(1))
		{
			RotateTowardOpponentDuringAction();
			animation.SetTrigger("PretendGuard(RightClick)");
		}
		else if (Input.GetKeyDown(KeyCode.Q)) // && cooldown done
		{
			if (bulletCooldown <= 0)
			{
				maxCharge = false;
				chargeBar.SetActive(true);
			}
		}
		else if (Input.GetKeyUp(KeyCode.Q))
		{
			if (bulletCooldown <= 0)
			{
				RotateTowardOpponentDuringAction();
				Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
				chargeBar.SetActive(false);

				bulletCooldown = bulletCooldownDuration;
			}
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			if (wallCooldown <= 0)
			{
				recordEndPos = OpponentChan.Instance.transform;

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
				RotateTowardOpponentDuringAction();
				animation.SetTrigger("Skill(ASDASD)");
				Instantiate(ultimate, ultimateSpawnPoint.position, ultimateSpawnPoint.rotation);

				ultimateCooldown = ultimateCooldownDuration;
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
		else
		{
			fillCharge.fillAmount = 0;
		}
	}

	void Cooldowns()
	{
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
}