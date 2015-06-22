using UnityEngine;
using System.Collections;

public class EnemyDiver: Enemy {

	public bool isDiving = false;
	public Vector3 nonTarget = Vector3.zero;
	public float diveDistance = 2f;
	public float wanderThreshold = 0.2f;

	void Start()
	{
		InitEnemy ();
		
		m_damage = 10f;
		m_speed = Random.value * m_speed + m_speed;
		
		GetNewNonTarget();
	}
	
	override public void UpdateMovement()
	{
		float dis = Vector3.Distance (m_playerTarget.transform.localPosition, this.transform.localPosition);
		if (dis < diveDistance) {
			nonTarget = m_playerTarget.transform.localPosition;
			isDiving = true;
		}


		Vector3 newPosition = this.transform.localPosition;

		if (isDiving) {
			newPosition = Vector3.Lerp (this.transform.localPosition, 
		                                    m_playerTarget.transform.localPosition,
			                   		        m_speed * M_SPEED_SCALE);

		}else{

			if (Vector3.Distance (nonTarget, this.transform.localPosition) < wanderThreshold)
				GetNewNonTarget();

			newPosition = Vector3.Lerp (this.transform.localPosition, 
			                            nonTarget,
			                            m_speed * M_SPEED_SCALE * 0.15f);
		}


		this.transform.localPosition = newPosition;
	}

	void GetNewNonTarget()
	{
		nonTarget = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range( 0f, Screen.width), Random.Range( 0f, Screen.height ), 0));
		nonTarget.Scale (new Vector3 (1f, 1f, 0f));
	}
}
