using UnityEngine;
using System.Collections;

public class EnemyChaser : Enemy_Base {

	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);

		m_speed = Random.value * m_speed + m_speed;
	}

	override public void UpdateMovement()
	{
		//TODO: Add some more personality in here
		float accScale = (m_accelerationScale + (Random.Range (0f, m_accelerationScale)));

		Vector3 target = m_playerTarget.transform.localPosition;
		m_acceleration = (target - m_position) * accScale;



		
		base.ApplyMovement (m_acceleration);
	}
}
