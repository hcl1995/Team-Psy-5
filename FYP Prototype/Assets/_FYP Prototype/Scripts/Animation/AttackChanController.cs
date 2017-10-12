using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackChanController : MonoBehaviour
{
	private static AttackChanController _instance;
	public static AttackChanController Instance
	{
		get 
		{
			if(_instance == null)
			{
				GameObject go = GameObject.Find("Attack_1");

				_instance = go.GetComponent<AttackChanController>();
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

	Vector3 startPos;
	Vector3 endPos;

	bool dash = false;
	float completeTime;
	float lerpSpeed = 10;

	//public Transform opponentChan;

//	public GameObject projectile;
//	public Transform spawnPoint;
//
//	public GameObject chargeBar;
//	public Image fillCharge;
//	public float chargeRate;
//
//	public bool maxCharge;
//
//	public GameObject wallSkill;
//	public GameObject wallSkill02;
//	public Transform wallSpawnPoint;
//	public Transform wallSpawnPoint02;
//	public Transform wallSpawnPoint03;
//	public Transform wallSpawnPoint04;

	[HideInInspector]
	public Transform recordEndPos;

	int attack;
	float attackInterval;
	public float attackIntervalLimit;

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

		Movement(x, z);
		Attack();
	}

	void Movement(float x, float z)
	{
		movement.Set (x, 0, z);
		movement = movement.normalized * speed * Time.deltaTime;
		rb.MovePosition (transform.position + movement);

		bool moving = x != 0f || z != 0f;
		//animation.SetBool("isMoving", moving);

		GetCameraRelativeMovement();
		RotateTowardMovementDirection();

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			z = dashing;

			startPos = transform.position;
			endPos = transform.position += (transform.forward * z);

			dash = true;
			trailRendererObject.transform.position = gameObject.transform.position;
			//transform.position += transform.forward * z;
			//animation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
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

//		if (Input.GetMouseButtonDown(0))
//		{
//			RotateTowardMouseDuringAction();
//			//animation.SetTrigger("Attack(LeftClick)");
//		}
//		else if (Input.GetMouseButtonDown(1))
//		{
//			RotateTowardMouseDuringAction();
//			//animation.SetTrigger("PretendGuard(RightClick)");
//		}
//		else if (Input.GetKeyDown(KeyCode.Q)) // && cooldown done
//		{
//			maxCharge = false;
//			chargeBar.SetActive(true);
//		}
//		else if (Input.GetKeyUp(KeyCode.Q))
//		{
//			RotateTowardOpponentDuringAction();
//			Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
//			chargeBar.SetActive(false);
//		}
//		else if (Input.GetKeyDown(KeyCode.E))
//		{
//			recordEndPos = OnHitChan.Instance.transform;
//
//			Instantiate(wallSkill, wallSpawnPoint.position, wallSpawnPoint.rotation);
//			Instantiate(wallSkill, wallSpawnPoint02.position, wallSpawnPoint02.rotation);
//			Instantiate(wallSkill, wallSpawnPoint03.position, wallSpawnPoint03.rotation);
//			Instantiate(wallSkill02, wallSpawnPoint04.position, wallSpawnPoint04.rotation);
//		}
//		else if (Input.GetKeyDown(KeyCode.Space))
//		{
//			RotateTowardOpponentDuringAction();
//			animation.SetTrigger("Skill(ASDASD)");
//		}

//		if (chargeBar.activeInHierarchy)
//		{
//			fillCharge.fillAmount += Time.deltaTime * chargeRate;
//
//			if (fillCharge.fillAmount >= 1)
//			{
//				maxCharge = true;
//			}
//		}
//		else
//		{
//			fillCharge.fillAmount = 0;
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

		if (Physics.Raycast (ray, out hit, 100f, LayerMask.GetMask("Ground")))
		{
			Quaternion rotation = Quaternion.LookRotation(hit.point);
			transform.rotation = rotation;
		}
	}

	void Attack()
	{
		if (Input.GetMouseButtonDown(0))
		{
			attack++;
			attackInterval = 0;
			//Debug.Log(attack);
		}

		if (attack >= 1 && attackInterval > attackIntervalLimit)
		{
			attack = 0;
			attackInterval = 0;
		}
		else if (attack >= 1 && attackInterval < attackIntervalLimit)
		{
			attackInterval += Time.deltaTime;
		}

		if (Input.GetMouseButtonDown(0) && attack == 1 && attackInterval < attackIntervalLimit)
		{
			RotateTowardMouseDuringAction();
			animation.SetTrigger("Attack01");
		}
		else if (Input.GetMouseButtonDown(0) && attack == 2 && attackInterval < attackIntervalLimit)
		{
			RotateTowardMouseDuringAction();
			animation.SetTrigger("Attack02");
		}
		else if (Input.GetMouseButtonDown(0) && attack >= 3 && attackInterval < attackIntervalLimit)
		{
			attack = 0;
			attackInterval = 0;

			RotateTowardMouseDuringAction();
			animation.SetTrigger("Attack03");
		}

		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).
			GetChild(5).gameObject.SetActive(true);
			animation.ResetTrigger("Attack01");
		}
		else
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).
			GetChild(5).gameObject.SetActive(false);
		}

		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack_2"))
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
			animation.ResetTrigger("Attack02");
		}
		else
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
		}

		if (this.animation.GetCurrentAnimatorStateInfo(0).IsName("Attack_3"))
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).
			GetChild(5).gameObject.SetActive(true);
			animation.ResetTrigger("Attack03");
		}
		else
		{
			transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).
			GetChild(5).gameObject.SetActive(false);
		}
	}
}