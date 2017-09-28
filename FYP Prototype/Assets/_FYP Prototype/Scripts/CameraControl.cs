using UnityEngine;

public class CameraControl : MonoBehaviour
{				
	public Transform player;
	public Transform target;

	public float dampTime = 0.2f;

	Vector3 moveVelocity;

	void Update()
	{
		float dist = Vector3.Distance(target.transform.position, player.transform.position);
		float yPos = (dist / 1);
		float xRot = (dist * 5); // 4

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

		Vector3 cam = new Vector3(((player.transform.position.x + target.transform.position.x) / 2), yPos, ((player.transform.position.z + target.transform.position.z) / 2) - 7);
		transform.position = Vector3.SmoothDamp(transform.position, cam, ref moveVelocity, dampTime);
		transform.rotation = Quaternion.Euler(xRot, 0, 0);
	}
}