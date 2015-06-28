using UnityEngine;
using System.Collections;

public class PlayerHelper : MonoBehaviour{
	public GameManager gameManager;
	public PlayerLife life;
	public PlayerMovement movement;

	public float m_collisionRadius = 1.0f;

	void Start()
	{
		if (life == null)
			life = this.GetComponent<PlayerLife> ();
		if (movement == null)
			movement = this.GetComponent<PlayerMovement> ();
	}

	void OnGUI()
	{
		Debug.DrawLine (new Vector3 (transform.position.x - m_collisionRadius, transform.position.y, transform.position.z), new Vector3 (transform.position.x + m_collisionRadius, transform.position.y, transform.position.z));
	}

	public void RegisterEnemyHit(Enemy_Base enemy)
	{
		movement.StartSleep ();
		life.TakeDamage (enemy.GetDamage ());
		Camera.main.GetComponent<CameraShake> ().StartShake ();
	}

	public void Reset()
	{
		movement.Reset ();
		life.Reset ();
	}

	public void PlayerDied()
	{
		if (gameManager != null)
			gameManager.PlayerDied ();
	}
}
