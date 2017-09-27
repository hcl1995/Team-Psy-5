using UnityEngine;

public class CamControl : MonoBehaviour
{
	public float m_DampTime = 0.2f;
	public float m_MinSize = 6.5f;
	public Transform[] m_Targets;

	private float m_ZoomSpeed;
	private Vector3 m_MoveVelocity;
	private Vector3 m_DesiredPosition;


	private void FixedUpdate ()
	{
		Move ();
		Zoom ();
	}
		
	private void Move ()
	{
		FindAveragePosition ();

		// Current camera position / Average position / How fast it currently moving to the speed now / How long its gonna take.
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += m_Targets[i].position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;
		// Keep the same y value.
		averagePos.x = transform.position.x;
		averagePos.y = transform.position.y;
		// averagePos.z -= 12.5f;

		// The desired position is the average position;
		m_DesiredPosition = averagePos;
	}
		
	private void Zoom ()
	{
		// Find the required size based on the desired position and smoothly transition to that size.
		float requiredSize = FindRequiredSize();
		float zoomSize = Mathf.SmoothDamp (transform.position.y, requiredSize, ref m_ZoomSpeed, m_DampTime);
		transform.position = new Vector3(transform.position.x, zoomSize, transform.position.z);
	}
		
	private float FindRequiredSize ()
	{
		// Find the position the camera rig is moving towards in its local space.
		Vector3 desiredLocalPos = transform.TransformPoint(m_DesiredPosition);

		// Start the camera's size calculation at zero.
		float size = 0f;

		// Go through all the targets...
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// ... and if they aren't active continue on to the next target.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Otherwise, find the position of the target in the camera's local space.
			Vector3 targetLocalPos = transform.TransformPoint(m_Targets[i].position);
			// Find the position of the target from the desired position of the camera's local space.
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
			// Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y)); // Mathf.Max = largest value.
			// Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / Camera.main.aspect);
		}
		size = Mathf.Max (size, m_MinSize);
		return size;
	}
}