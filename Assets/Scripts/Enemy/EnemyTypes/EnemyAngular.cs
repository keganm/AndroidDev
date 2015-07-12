using UnityEngine;
using System.Collections;

public class EnemyAngular : Enemy_Base {
	
	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);
	}
	
	override public void UpdateMovement()
	{
		//TODO: Add some more personality in here
		float accScale = (m_accelerationScale + (Random.Range (0f, m_accelerationScale)));

		float xDiff = m_playerTarget.transform.localPosition.x - m_position.x;
		float yDiff = m_playerTarget.transform.localPosition.y - m_position.y;

		
		Vector3 target = new Vector3(m_position.x, m_playerTarget.transform.localPosition.y, 0f);
		if (Mathf.Abs (xDiff) > Mathf.Abs (yDiff))
			target = new Vector3 (m_playerTarget.transform.localPosition.x, m_position.y, 0f);
 		
		m_acceleration = (target - m_position) * accScale;
		
		
		
		
		base.ApplyMovement (m_acceleration);
	}
}
