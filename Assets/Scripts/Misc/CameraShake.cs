using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public float m_ShakeTime = 0f;
	public float m_ShakeTimeStep = 0.1f;
	public float m_ShakeMagnitude = 2f;
	public float m_Smoothness = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_ShakeTime > 0f) {

			Vector2 rand = Random.insideUnitCircle * (m_ShakeMagnitude * m_ShakeTime);
			Vector3 newPos = new Vector3(rand.x,rand.y,this.transform.localPosition.z);
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition,newPos,m_Smoothness);

			m_ShakeTime -= m_ShakeTimeStep;

		}
	}

	public void StartShake()
	{
		StartShake (0.05f, 1f, 0.5f);
	}

	public void StartShake(float timestep, float shakemagnitude, float smoothness)
	{
		m_ShakeTime = 1f;
		m_ShakeTimeStep = timestep;
		m_ShakeMagnitude = shakemagnitude;
		m_Smoothness = smoothness;
	}
}
