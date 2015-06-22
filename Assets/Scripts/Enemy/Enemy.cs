using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	GuiTextDebug textDebug;
	//General Variables
	public float M_SPEED_SCALE = 0.5f;

	//Local
	public GameObject m_playerTarget;

	public float m_damage;
	public float m_speed; //normalized 0..1

	public bool m_isAwake;
	public bool m_isAlive;
	public bool m_isDying;

	private ParticleSystem pSystem;

	public virtual void InitEnemy()
	{
		pSystem = this.GetComponentInChildren<ParticleSystem> ();
		textDebug = Camera.main.GetComponent<GuiTextDebug> ();

		m_isAwake = true;
		m_isAlive = true;
		FindTarget ();
	}

	public virtual float GetDamage()
	{
		return m_damage;
	}

	public virtual bool IsAlive()
	{
		return m_isAlive;
	}

	public virtual bool IsAwake()
	{
		return m_isAwake;
	}

	public virtual void Kill()
	{
		m_isAwake = false;
		StartDying ();
	}

	public virtual void FindTarget()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		float distance = 1000f;

		foreach (GameObject player in players) {
			float d = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);

			if(d < distance)
			{
				distance = d;
				m_playerTarget = player;
			}
		}
	}

	public virtual void Update()
	{
		if (m_isDying) {
			DyingSequence();
			return;
		}

		if (!m_isAwake)
			return;

		UpdateMovement ();
	}

	public virtual void UpdateMovement()
	{

	}

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		if(textDebug.debugIsOn)
			textDebug.Log ("Enemy Contact:" + other.transform.name);

		if (other.gameObject.tag == "Player") {
			m_isAlive = false;
			other.gameObject.GetComponent<AndroidInput.solidTouch>().StartSleep();
		}
	}

	public virtual void StartDying()
	{
		m_isDying = true;
		this.GetComponentInChildren<MeshRenderer> ().enabled = false;
		pSystem.Emit (1000);
	}

	public virtual void DyingSequence()
	{
		if (pSystem.particleCount < 10) {
			DestroyImmediate(this.gameObject);
		}
	}
}
