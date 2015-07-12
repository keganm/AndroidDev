using UnityEngine;
using System.Collections;

public class EnemyChaser : Enemy_Base {

	override public void InitEnemy(EditRect _rect)
	{
		m_mass = m_mass + Random.Range (0f, m_massRandomness);

		base.InitEnemy (_rect);
	}

	override public void UpdateMovement()
	{
		//TODO: Add some more personality in here
		float accScale = m_accelerationScale * m_mass;

		Vector3 target = m_playerTarget.transform.localPosition;
		m_acceleration = (target - m_position) * accScale;



		
		base.ApplyMovement (m_acceleration);
	}
}
