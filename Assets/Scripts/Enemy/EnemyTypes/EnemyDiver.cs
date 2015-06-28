using UnityEngine;
using System.Collections;

public class EnemyDiver: Enemy_Base {

	public bool isDiving = false;
	public Vector3 nonTarget = Vector3.zero;
	public float diveDistance = 2f;
	public float wanderThreshold = 0.2f;
	
	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);
		m_speed = Random.value * m_speed + m_speed;
		
		GetNewNonTarget();
	}
	
	override public void UpdateMovement()
	{
		float dis = Vector3.Distance (m_playerTarget.transform.localPosition, this.transform.localPosition);
		float accScale = (m_accelerationScale + (Random.Range (0f, m_accelerationScale)));
		if (dis < diveDistance) {
			nonTarget = m_playerTarget.transform.localPosition;
			isDiving = true;
		}

		if (isDiving) {
			
			Vector3 target = m_playerTarget.transform.localPosition;
			m_acceleration = (target - m_position) * accScale;

		}else{

			GetNewNonTarget();
			
			Vector3 target = m_playerTarget.transform.localPosition;
			m_acceleration = (nonTarget - m_position) * accScale;


		}

		base.ApplyMovement (m_acceleration);
	}

	void GetNewNonTarget()
	{
		if (Vector3.Distance (nonTarget, m_position) < wanderThreshold)
			nonTarget = RandContainerPosition ();
	}
}
