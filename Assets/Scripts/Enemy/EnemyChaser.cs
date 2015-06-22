using UnityEngine;
using System.Collections;

public class EnemyChaser : Enemy {

	void Start()
	{
		InitEnemy ();

		m_damage = 10f;
		m_speed = Random.value * m_speed + m_speed;
	}

	override public void UpdateMovement()
	{
		Vector3 newPosition = Vector3.Lerp (this.transform.localPosition, 
		                                    m_playerTarget.transform.localPosition,
		                                    m_speed * M_SPEED_SCALE);

		this.transform.localPosition = newPosition;
	}
}
