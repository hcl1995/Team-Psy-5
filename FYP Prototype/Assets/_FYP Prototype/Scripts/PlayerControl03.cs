using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerControl03 : NetworkBehaviour
{
	private static PlayerControl03 _instance;
	public static PlayerControl03 Instance
	{
		get 
		{
			if(_instance == null)
			{
				GameObject go = GameObject.FindObjectOfType<PlayerControl03>().gameObject;

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
	NetworkAnimator nAnimation;

	public float speed;
	public float dashing;

	public Transform opponentChan;

	public GameObject projectile;
	public Transform spawnPoint;

	public GameObject chargeBar;
	public Image fillCharge;
	public float chargeRate;

	public bool maxCharge;

	public GameObject wallSkill;
	public Transform wallSpawnPoint;
	public GameObject wallSkill02;
	public Transform wallSpawnPoint02;
	public GameObject wallSkill03;
	public Transform wallSpawnPoint03;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
		nAnimation = GetComponent<NetworkAnimator> ();

	}

	void Start()
	{

	}

	void Update()
	{
		if (isLocalPlayer) {
			CheckInput ();
		}
	}

	void CheckInput(){
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");

		movement = new Vector3(x, 0, z);

		Movement(x, z);
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
			transform.Translate (movement + (Vector3.forward * z));
			nAnimation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
		}

		if (Input.GetMouseButtonDown(0))
		{
			RotateTowardOpponentDuringAction();
			//animation.SetTrigger("Attack(LeftClick)");
			CmdFire (spawnPoint.position,spawnPoint.rotation);
		}
		else if (Input.GetMouseButtonDown(1))
		{
			RotateTowardOpponentDuringAction();
			nAnimation.SetTrigger("PretendGuard(RightClick)");
		}
		else if (Input.GetKeyDown(KeyCode.Q)) // && cooldown done
		{
			maxCharge = false;
			chargeBar.SetActive(true);
		}
		else if (Input.GetKeyUp(KeyCode.Q))
		{
			RotateTowardOpponentDuringAction();
			chargeBar.SetActive(false);
			CmdFire (spawnPoint.position,spawnPoint.rotation);
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			Instantiate(wallSkill, wallSpawnPoint.position, wallSpawnPoint.rotation);
			Instantiate(wallSkill02, wallSpawnPoint02.position, wallSpawnPoint02.rotation);
			Instantiate(wallSkill03, wallSpawnPoint03.position, wallSpawnPoint03.rotation);
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			RotateTowardOpponentDuringAction();
			nAnimation.SetTrigger("Skill(ASDASD)");
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

	[Command]
	void CmdFire(Vector3 position, Quaternion rotation){
		var bullet = Instantiate(projectile, position, rotation);
		NetworkServer.Spawn (bullet);
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
}
