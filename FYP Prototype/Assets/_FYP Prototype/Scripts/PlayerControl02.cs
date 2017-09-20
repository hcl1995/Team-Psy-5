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

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
	}

	void Update()
	{
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
			animation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			animation.SetTrigger("Attack(LeftClick)");
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			animation.SetTrigger("PretendGuard(RightClick)");
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.U)) // && cooldown done
		{
			animation.SetTrigger("Skill(SWW)");
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			animation.SetTrigger("Skill(ASD)");
			transform.LookAt(opponentChan);
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			animation.SetTrigger("Skill(ASDASD)");
			transform.LookAt(opponentChan);
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