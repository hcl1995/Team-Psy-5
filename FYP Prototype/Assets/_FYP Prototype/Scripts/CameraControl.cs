using UnityEngine;

public class CameraControl : MonoBehaviour
{				
	public Transform player;
	public Transform target;

	public float dampTime = 0.2f;

	Vector3 moveVelocity;

	void Update()
	{
		// try the average position as well.
		float dist = Vector3.Distance(target.transform.position, player.transform.position);
		float yPos = (dist / 1.5f);
		float xRot = (dist * 4.0f);

		if (yPos <= 3.5)
		{
			yPos = 3.5f;
		}
		else if (yPos >= 8.5)
		{
			yPos = 8.5f;
		}

		if (xRot <= 40)
		{
			xRot = 40f;
		}
		else if (xRot >= 60)
		{
			xRot = 60f;
		}

		Vector3 cam = new Vector3(0, yPos, player.transform.position.z - 3.75f);
		transform.position = Vector3.SmoothDamp(transform.position, cam, ref moveVelocity, dampTime);
		// transform.position = new Vector3(0, yPos, player.transform.position.z - 3.75f);

		//transform.rotation = Quaternion.SmoothDamp(transform.position, cam, ref moveVelocity, dampTime);
		transform.rotation = Quaternion.Euler(xRot, 0, 0);
	}
}