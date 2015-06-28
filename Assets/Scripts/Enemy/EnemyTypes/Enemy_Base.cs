using UnityEngine;
using System.Collections;

public class Enemy_Base : MonoBehaviour {
	//GuiTextDebug textDebug;
	//General Variables
	public EnemySpawner M_SPAWNER;

	//Local
	public float m_accelerationScale = 0.5f;
	public Vector3 m_acceleration;
	public Vector3 m_velocity;
	public Vector3 m_position;

	public Vector3 m_Gravity = new Vector3(0f,0f,0f);
	public Vector3 m_Drag = new Vector3(1.0f,1.0f,1.0f);

	public GameObject m_playerTarget;

	public float m_speed; //normalized 0..1
	public float m_maxVelocity = 10f;
	
	public float m_damage;

	public bool m_isAwake;
	public bool m_isAlive;
	public bool m_isDying = false;

	public int m_maxChildren = 25;
	public bool m_isDestroyingChildren = false;

	private ParticleSystem pSystem;

	private Rect m_containerRect;
	public Rect ContainerRect {
		get{ return m_containerRect; }
		set{ m_containerRect = value; }
	}
	public EditRect m_editRect;

	public float m_collisionRadius = 0.5f;

	public virtual void InitEnemy(EditRect _rect)
	{
		M_SPAWNER = this.GetComponentInParent<EnemySpawner> ();
		pSystem = this.GetComponentInChildren<ParticleSystem> ();
		//textDebug = Camera.main.GetComponent<GuiTextDebug> ();

		m_editRect = _rect;
		this.ContainerRect = m_editRect.m_Rectangle;
		
		m_isAwake = true;
		m_isAlive = true;
		FindTarget ();

		m_position = this.transform.localPosition;
	}

	public virtual void Reset()
	{
		if(this != null)
			GameObject.DestroyImmediate (this.gameObject);
	}

	public virtual void Kill()
	{
		m_isAlive = false;
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
		if (Time.timeScale <= 0f)
			return;

		if (m_isDestroyingChildren){
			DestroyChildren ();
			return;
		}
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

	public virtual void ApplyMovement(Vector3 accl)
	{

		//float angle = Mathf.Rad2Deg * ( Mathf.Atan2 (target.y - this.transform.localPosition.y, target.x - this.transform.localPosition.x));
		//this.transform.localRotation = Quaternion.Euler( new Vector3(0f,0f,angle) );
		this.transform.localRotation = Quaternion.LookRotation (this.transform.forward,m_velocity.normalized);
		Vector3 vel = m_velocity;
		vel.Scale (m_Drag);
		vel += m_Gravity;

		vel += accl;
		vel = Vector3.ClampMagnitude (vel, m_maxVelocity);
		m_velocity = vel;
		
		//Vector3 vel = Vector3.ClampMagnitude (target - this.transform.localPosition, m_maxVelocity);
		Vector3 pos = m_position + m_velocity;
		if (ContainerRect.Contains (pos)) {
			m_position = pos;
			this.transform.localPosition = m_position;
		}
		//	this.transform.localPosition += new Vector3(vel.x,vel.y,0f);

		CheckPlayerCollision ();
	}

//	public virtual void OnTriggerEnter2D(Collider2D other)
//	{
//		if (m_isDying || m_isDestroyingChildren)
//			return;
//
//		PlayerHelper player = other.gameObject.GetComponent<PlayerHelper> ();
//		if (player != null) {
//			Kill();
//
//			player.RegisterEnemyHit(this);
//		}
//	}

	public virtual void CheckPlayerCollision()
	{
		if (m_isDying || m_isDestroyingChildren)
			return;

		PlayerHelper player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHelper> ();
		float dist = Mathf.Pow (this.m_collisionRadius + player.m_collisionRadius, 2);
		Vector3 offset = player.transform.position - this.transform.position;
		if (offset.sqrMagnitude < dist) {
			Kill();
			player.RegisterEnemyHit (this);
			Debug.Log ("Collision Registered");
		}
	}



	public virtual void StartDying()
	{
		if (m_isDying)
			return;
		
		m_isAwake = false;
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

	public virtual void DestroyChildren()
	{

	}

	
	public virtual Vector3 RandContainerPosition()
	{
		return new Vector3 (Random.Range (ContainerRect.xMin, ContainerRect.xMax), Random.Range (ContainerRect.yMin, ContainerRect.yMax), 0f);
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
}
