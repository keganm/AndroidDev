using UnityEngine;
using System.Collections;

public class EnemyDiver: Enemy_Base {

	protected bool m_isDiving = false;
	protected Vector3 m_nonTarget = Vector3.zero;
	[SerializeField]	protected float m_diveDistance = 2f;
	[SerializeField]	protected float m_wanderThreshold = 0.2f;
	
	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);
		GetNewNonTarget();
	}
	
	override public void UpdateMovement()
	{
		if (m_playerTarget == null) {
			if(!FindTarget())
				return;
		}

		float dis = Vector3.Distance (m_playerTarget.transform.localPosition, this.transform.localPosition);
		float accScale = (m_accelerationScale + (Random.Range (0f, m_accelerationScale)));
		if (dis < m_diveDistance) {
			m_nonTarget = m_playerTarget.transform.localPosition;
			m_isDiving = true;
		}

		if (m_isDiving) {
			
			Vector3 target = m_playerTarget.transform.localPosition;
			m_acceleration = (target - m_position) * accScale;

		}else{

			GetNewNonTarget();
			m_acceleration = (m_nonTarget - m_position) * accScale;


		}

		base.ApplyMovement (m_acceleration);
	}

	void GetNewNonTarget()
	{
		if (Vector3.Distance (m_nonTarget, m_position) < m_wanderThreshold)
			m_nonTarget = RandContainerPosition ();
	}
}
