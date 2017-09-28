using UnityEngine;

public class CameraControlFix : MonoBehaviour
{
	public float m_DampTime;
	public float m_ScreenEdgeBuffer;
	public float m_MinSize;
	public Transform[] m_Targets;

	float distance;
	float m_ZoomSpeed;
	Vector3 m_MoveVelocity;
	Vector3 m_DesiredPosition;

	void FixedUpdate()
	{
		Vector3 averagePos = new Vector3();

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			averagePos += m_Targets[i].position;

			distance = Vector3.Distance(m_Targets[0].position, m_Targets[1].position);
		}

		averagePos /= 2;
		m_DesiredPosition = averagePos;

		float xRot = (distance * 5);

		if (xRot <= 40)
		{
			xRot = 40f;
		}
		else if (xRot >= 60)
		{
			xRot = 60f;
		}

		transform.rotation = Quaternion.Euler(xRot, 0, 0);

		float requiredSize = FindRequiredSize();
		float zoomYPosition = Mathf.SmoothDamp(transform.position.y, requiredSize, ref m_ZoomSpeed, m_DampTime);

		Vector3 desiredPosition = new Vector3(averagePos.x, zoomYPosition, averagePos.z);
		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_MoveVelocity, m_DampTime);
	}
		
	float FindRequiredSize()
	{
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		float size = 0f;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.z));
			size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / Camera.main.aspect);
		}
		size += m_ScreenEdgeBuffer;
		size = Mathf.Max(size, m_MinSize);
		return size;
	}
}