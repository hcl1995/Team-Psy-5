using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	Rigidbody rb;
	Animator animation;

	Vector3 movement;

	private KeyCombo skillSWW = new KeyCombo(new string[] {"down", "up", "up", "Fire1"});
	private KeyCombo skillASD = new KeyCombo(new string[] {"left", "down", "right", "Fire1"});
	private KeyCombo skillASDASD = new KeyCombo(new string[] {"left", "down", "right", "left", "down", "right", "Fire1"});

	public float speed;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		animation = GetComponent<Animator>();
	}

	void Update()
	{
		float x = 0;
		float z = 0;

		if (Input.GetKey(KeyCode.W))
		{
			z = 1;
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				z = 20;
				transform.Translate (movement + (Vector3.forward * z));
				animation.SetTrigger("Dash/Tumble(Shift)"); // Stamina / Charges
			}
		}
		if (Input.GetKey(KeyCode.S))
		{
			z = -1;
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				z = -20;
				transform.Translate (movement + (Vector3.forward * z));
				animation.SetTrigger("Dash/Tumble(Shift)");
			}
		}
		if (Input.GetKey(KeyCode.A))
		{
			x = -1;
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				x = -20;
				transform.Translate (movement + (Vector3.right * x));
				animation.SetTrigger("Dash/Tumble(Shift)");
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			x = 1;
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				x = 20;
				transform.Translate (movement + (Vector3.right * x));
				animation.SetTrigger("Dash/Tumble(Shift)");
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			animation.SetTrigger("Attack(LeftClick)");
		}
		else if (Input.GetMouseButtonDown(1))
		{
			animation.SetTrigger("PretendGuard(RightClick)");
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			CheckCombo();
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			CheckCombo();
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			CheckCombo();
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			CheckCombo();
		}
		else if (Input.GetButtonDown("Fire1"))
		{
			CheckCombo();
		}

		Movement(x, z);
	}

	void Movement(float x, float z)
	{
		movement.Set (x, 0, z);
		//normalize is a must.. require normalize + dash..
		movement = movement.normalized * speed * Time.deltaTime;
		//movement = movement * speed * Time.deltaTime;
		transform.Translate (movement);

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f))
		{
			Vector3 playerToMouse = hit.point - transform.position;
			playerToMouse.y = 0f;

			// transform.LookAt(hit.point);
			Quaternion rotation = Quaternion.LookRotation(playerToMouse);
			transform.rotation = rotation;
		}

		bool moving = x != 0f || z != 0f;
		animation.SetBool("isMoving", moving);
	}

	void CheckCombo()
	{
		if (skillSWW.Check())
		{
			Debug.Log("SWW"); 
			animation.SetTrigger("Skill(SWW)");
		}
		if (skillASD.Check())
		{
			Debug.Log("ASD");
			animation.SetTrigger("Skill(ASD)");
		}
		if (skillASDASD.Check())
		{
			Debug.Log("ASDASD");
			animation.SetTrigger("Skill(ASDASD)");
		}
	}

//	void Attack()
//	{
//		int attack = 0 ;
//		float attackInterval = 0;
//
//		if (Input.GetKeyDown(KeyCode.S))
//		{
//			attack += 1;
//			animation.SetTrigger("Skill(SWW)"); // Attack01
//		}
//
//		if (attack == 1 && attackInterval < 0.5)
//		{
//			attackInterval += Time.deltaTime;
//		}
//		if (attack == 1 && attackInterval > 0.5)
//		{
//			attack = 0;
//			attackInterval = 0;
//		}
//
//		if (attack == 2 && attackInterval < 0.5)
//		{
//			attack = 0;
//			attackInterval = 0;
//			animation.SetTrigger("Skill(SWW)"); // Attack02
//		}
//	}
}