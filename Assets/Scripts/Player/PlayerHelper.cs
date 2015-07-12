using UnityEngine;
using System.Collections;

public class PlayerHelper : MonoBehaviour{
	public GameController gameController;
	public PlayerLife life;
	public PlayerMovement movement;
	public ShowDamage showDamage;

	public float m_collisionRadius = 1.0f;

	void Start()
	{
		if (gameController == null) {
			gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
			if(gameController!= null)
				gameController.m_playerHelper = this;
		}

		if (life == null)
			life = this.GetComponent<PlayerLife> ();
		if (movement == null)
			movement = this.GetComponent<PlayerMovement> ();
		if (showDamage == null)
			showDamage = this.GetComponent<ShowDamage> ();
	}

	public void RegisterEnemyHit(Enemy_Base enemy)
	{
		movement.StartSleep ();
		life.TakeDamage (enemy.Damage);
		showDamage.StartDamage ();
		CameraShake cs = Camera.main.GetComponent<CameraShake> ();
		if(cs != null)
			cs.StartShake ();
	}

	public void Reset()
	{
		movement.Reset ();
		life.Reset ();
	}

	public void PlayerDied()
	{
		if (gameController != null)
			gameController.PlayerDied ();
	}
}
