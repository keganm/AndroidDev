using UnityEngine;
using System.Collections;

public class Enemy_Base : MonoBehaviour {
	public EnemySpawner kSPAWNER;
	public GameObject m_playerTarget;
	
	public ParticleSystem m_deathParticleSystem;
	public ParticleSystem m_spawnParticleSystem;
	
	//Local Behaviour Properties
	
	protected Vector3 m_acceleration;
	protected Vector3 m_velocity;
	protected Vector3 m_position;
	protected Vector3 Position {
		get{ return m_position; }
		set{ m_position = value;}
	}
	
	[SerializeField]	protected float m_accelerationScale = 0.5f;
	[SerializeField]	protected float m_collisionRadius = 0.5f;
	[SerializeField]	protected float m_mass = 1.0f;
	[SerializeField]	protected float m_massRandomness = 0.0f;
	
	[SerializeField]	protected float m_maxVelocity = 2f;
	[SerializeField]	protected Vector3 m_Gravity = new Vector3(0f,0f,0f);
	[SerializeField]	protected Vector3 m_Drag = new Vector3(1.0f,1.0f,1.0f);

	
	[SerializeField]	protected float m_spawnParticleFalloff = 0.95f;
	
	[SerializeField]	protected float m_damage;
	public float Damage {
		get{ return m_damage; }
		set{ m_damage = value;}
	}
	
	//Control children of children
	[SerializeField]	protected int m_maxChildren = 25;
	// Local States
	protected bool m_isSpawning;
	protected bool m_isAwake;
	protected bool m_isAlive;
	protected bool m_isDying = false;
	
	public bool IsSpawning {get{ return m_isSpawning; }}
	public bool IsAwake {get{ return m_isAwake; }}
	public bool IsAlive {get{ return m_isAlive; }}
	public bool IsDying {get{ return m_isDying; }}
	
	protected bool m_isDestroyingChildren = false;
	
	//Rectangle method for spawning and creating pulls gameobject with "BoundingBox" tag
	protected EditRect m_editRect;
	private Rect m_containerRect;
	public Rect ContainerRect {
		get{ return m_containerRect; }
		set{ m_containerRect = value; }
	}
	
	
	public virtual void InitEnemy(EditRect _rect)
	{
		kSPAWNER = this.GetComponentInParent<EnemySpawner> ();
		
		m_editRect = _rect;
		this.ContainerRect = m_editRect.m_Rectangle;
		
		m_isSpawning = true;
		m_isAwake = true;
		m_isAlive = true;
		FindTarget ();
		
		m_position = this.transform.localPosition;
	}
	
	public virtual void Reset()
	{
	}
	
	/// <summary>
	/// Kill this enemy.
	/// Goes into dying sequence
	/// </summary>
	public virtual void Kill()
	{
		m_isAlive = false;
		StartDying ();
	}
	
	
	public virtual void FindTarget(GameObject _target)
	{
		m_playerTarget = _target;
	}
	
	public virtual bool FindTarget()
	{
		return FindTarget (1000f);
	}
	
	/// <summary>
	/// Finds the nearest player within max distance.
	/// </summary>
	/// <param name="maxDistance">Max distance.</param>
	public virtual bool FindTarget(float maxDistance)
	{
		bool foundplayer = false;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		
		float distance = maxDistance;
		
		foreach (GameObject player in players) {
			float d = Vector3.Distance(this.transform.localPosition, player.transform.localPosition);
			
			if(d < distance)
			{
				distance = d;
				m_playerTarget = player;
				foundplayer = true;
			}
		}
		
		return foundplayer;
	}
	
	public virtual void Update()
	{
		if (Time.timeScale <= 0f)
			return;
		
		if(m_isSpawning)
			m_isSpawning = SpawningEffect ();
		
		if (HandleDying ())
			return;
		
		if (!m_isAwake)
			return;
		
		UpdateMovement ();
		
		CheckPlayerCollision ();
	}
	
	/// <summary>
	/// Handles spawning effect of enemy.
	/// </summary>
	/// <returns><c>true</c>, if effect is active, <c>false</c> otherwise.</returns>
	public virtual bool SpawningEffect()
	{
		if (m_spawnParticleSystem == null)
			return false;
		if (m_spawnParticleSystem.emissionRate > 5) {
			m_spawnParticleSystem.emissionRate *= m_spawnParticleFalloff;
			return true;
		} else {
			m_spawnParticleSystem.emissionRate = 0;
			return false;
		}
	}
	
	/// <summary>
	/// Handles the dying process.
	/// </summary>
	/// <returns><c>true</c>, if enemy is dying, <c>false</c> otherwise.</returns>
	public virtual bool HandleDying()
	{
		if (m_isDestroyingChildren){
			DestroyChildren ();
			return true;
		}
		if (m_isDying) {
			DyingSequence();
			return true;
		}
		return false;
	}
	
	public virtual void UpdateMovement (){}
	
	public virtual void ApplyMovement(Vector3 accl)
	{
		this.transform.localRotation = Quaternion.LookRotation (this.transform.forward,m_velocity.normalized);
		Vector3 vel = m_velocity;
		vel.Scale (m_Drag);
		vel += m_Gravity;
		
		vel += accl;
		vel = Vector3.ClampMagnitude (vel, m_maxVelocity);
		m_velocity = vel;
		
		Vector3 pos = m_position + m_velocity;
		if (ContainerRect.Contains (pos)) {
			m_position = pos;
			this.transform.localPosition = m_position;
		}
		
	}
	
	/// <summary>
	/// Checks for player collision.
	/// </summary>
	public virtual void CheckPlayerCollision()
	{
		if (m_isDying || m_isDestroyingChildren)
			return;
		
		if (m_playerTarget == null) {
			if(!FindTarget()){
				Debug.Log("Enemy: " + this.gameObject.name + " could not find player target");
				return;
			}
		}
		
		PlayerHelper player = m_playerTarget.GetComponent<PlayerHelper> ();
		if (player == null) {
			m_playerTarget = null;
			return;
		}
		
		float dist = Mathf.Pow (this.m_collisionRadius + player.m_collisionRadius, 2);
		
		Vector3 offset = player.transform.position - this.transform.position;
		if (offset.sqrMagnitude < dist) {
			Kill();
			player.RegisterEnemyHit (this);
			Debug.Log ("Collision Registered");
		}
	}
	
	
	/// <summary>
	/// Start dying.
	/// Puts to sleep and hides geometry, and calls death effect
	/// Default it emission of Death Particles
	/// </summary>
	public virtual void StartDying()
	{
		if (m_isDying)
			return;
		
		m_isAwake = false;
		m_isDying = true;
		
		this.GetComponentInChildren<MeshRenderer> ().enabled = false;
		StartDeathEffect ();
	}
	
	public virtual void StartDeathEffect()
	{
		m_deathParticleSystem.Emit (1000);
	}
	
	public virtual void DyingSequence()
	{
		if (m_deathParticleSystem.particleCount < 10) {
			DestroyImmediate(this.gameObject);
		}
	}
	
	/// <summary>
	/// Only needs to be in enemy sub classes that spawn children
	/// </summary>
	public virtual void DestroyChildren()
	{
	}
	
	
	public virtual Vector3 RandContainerPosition()
	{
		return new Vector3 (Random.Range (ContainerRect.xMin, ContainerRect.xMax), Random.Range (ContainerRect.yMin, ContainerRect.yMax), 0f);
	}
	
	
}
