using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine to shake the camera
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class CameraShake : CinemachineExtension
{
	[Tooltip("Amplitude of the shake")]
	public float m_Range;
	public float m_shakeTime;
	public float m_shakeDuration;
	public GameObject brokenLava;
	static bool shakeIt = true;

	void Start()
	{
		shakeIt = true;
	}

	protected override void PostPipelineStageCallback(
		CinemachineVirtualCameraBase vcam,
		CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		brokenLava = GameObject.FindGameObjectWithTag("CameraShaker");

		if (brokenLava == null)
			return;

		if (brokenLava.activeInHierarchy)
		{
			if (stage == CinemachineCore.Stage.Body && m_shakeTime <= m_shakeDuration && shakeIt)
			{
				m_shakeTime += Time.deltaTime;
				Vector3 shakeAmount = GetOffset();
				state.PositionCorrection += shakeAmount;
			}

			if (m_shakeTime >= m_shakeDuration)
			{
				shakeIt = false;
			}
		}
	}

	Vector3 GetOffset()
	{
		return new Vector3(Random.Range(-m_Range, m_Range), Random.Range(-m_Range, m_Range), Random.Range(-m_Range, m_Range));
	}
}