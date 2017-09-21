using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl02 : MonoBehaviour
{
	float rotationSpeed = 30;

	Vector3 movement;
	Vector3 targetDirection;

	Rigidbody rb;
	Animator animation;

	public float speed;
	public float dashing;

	public Transform opponentChan;

	private PhotonView PhotonView;
	public bool UseTransformView = true;
	private Quaternion TargetRotation;
	private Vector3 TargetPosition;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
		PhotonView = GetComponent<PhotonView>();
		opponentChan = FindObjectOfType<CrazyRotate> ().transform;
	}
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (UseTransformView)
			return;

		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else
		{
			TargetPosition = (Vector3)stream.ReceiveNext();
			TargetRotation = (Quaternion)stream.ReceiveNext();
		}
	}
	void Update()
	{
		if (PhotonView.isMine)
			CheckInput();
		else
			SmoothMove();
	}
	private void SmoothMove()
	{
		if (UseTransformView)
			return;

		transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
	}
	private void CheckInput(){
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

		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K))
		{
			z = dashing;
			transform.Translate (movement + (Vector3.forward * z));
			PhotonView.RPC("RPC_PerformDash", PhotonTargets.All);
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			PhotonView.RPC("RPC_PerformAttack", PhotonTargets.All);
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			PhotonView.RPC("RPC_PerformGuard", PhotonTargets.All);
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.U)) // && cooldown done
		{
			PhotonView.RPC("RPC_PerformSkill01", PhotonTargets.All);
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			PhotonView.RPC("RPC_PerformSkill02", PhotonTargets.All);
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			PhotonView.RPC("RPC_PerformSkill03", PhotonTargets.All);
			transform.LookAt(opponentChan);
		}
	}

	[PunRPC]
	private void RPC_PerformDash()
	{
		animation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
	}

	[PunRPC]
	private void RPC_PerformAttack()
	{
		animation.SetTrigger("Attack(LeftClick)"); // Stamina / Charges
	}

	[PunRPC]
	private void RPC_PerformGuard()
	{
		animation.SetTrigger("PretendGuard(RightClick)"); // Stamina / Charges
	}

	[PunRPC]
	private void RPC_PerformSkill01()
	{
		animation.SetTrigger("Skill(SWW)"); // Stamina / Charges
	}
	[PunRPC]
	private void RPC_PerformSkill02()
	{
		animation.SetTrigger("Skill(ASD)"); // Stamina / Charges
	}
	[PunRPC]
	private void RPC_PerformSkill03()
	{
		animation.SetTrigger("Skill(ASDASD)"); // Stamina / Charges
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

//	void RotateTowardOpponentDuringAction()
//	{
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//
//		if (Physics.Raycast (ray, out hit, 100f))
//		{
//			Vector3 playerToMouse = hit.point - transform.position;
//			playerToMouse.y = 0f;
//
//			// transform.LookAt(hit.point);
//			Quaternion rotation = Quaternion.LookRotation(playerToMouse);
//			transform.rotation = rotation;
//		}
//	}
}