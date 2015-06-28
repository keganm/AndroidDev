using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySplitter : Enemy_Base {
	public int retargetTimer = 50;
	public int retargetTime = 50;
	public int splitTimer = 100;
	public int splitTime = 0;
	
	public Vector3 nonTarget = Vector3.zero;

	public EnemySplitter parentEnemy = null;
	public List<EnemySplitter> childrenEnemies = new List<EnemySplitter>();

	private Vector3 targetArea;
	public float targetRadius = 0.5f;
	
	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);
		m_speed = Random.value * m_speed + m_speed;

		ReTarget ();

		targetArea = RandContainerPosition ();
	}

	override public void Reset()
	{
		for (int i = 0; i < childrenEnemies.Count; ++i) {
			childrenEnemies[i].Reset();
		}
		childrenEnemies.Clear ();
		base.Reset ();
	}
	
	override public void UpdateMovement()
	{
		ReTarget ();
		SplitEnemy ();

		m_acceleration = (nonTarget - m_position) * m_accelerationScale;
		
		base.ApplyMovement (m_acceleration);


	}

	private void ReTarget()
	{

		retargetTime++;

		if (retargetTime < retargetTimer)
			return;

		retargetTime = 0;

		if (parentEnemy == null) {


			Vector3 nta = RandContainerPosition();

			targetArea = Vector3.Lerp(targetArea, nta, 0.1f);
		}

		EnemySplitter _parent = GetParentSplitter ();
		float radius = ((float)_parent.childrenEnemies.Count / (float)m_maxChildren) * targetRadius;
		Vector2 area = Random.insideUnitCircle * radius;
		Vector2 size = ContainerRect.size;
		area.Scale (Camera.main.ScreenToWorldPoint( size ));

		Vector3 centerRandom = GetParent().GetComponent<EnemySplitter>().targetArea;
		Vector3 newTarget = new Vector3 (centerRandom.x + area.x, centerRandom.y + area.y, 0f);

		//newTarget = RandContainerWorldPosition ();
		nonTarget = Vector3.Lerp(nonTarget, newTarget, 0.5f);
	}

	private void SplitEnemy()
	{
		splitTime++;

		if (splitTime < splitTimer || M_SPAWNER == null)
			return;
		
		splitTime = 0;

		if (MaximumChildren ())
			return;
		
		GameObject newEnemy = GameObject.Instantiate (this.gameObject);
		SplitEnemy (newEnemy);
	}

	private void SplitEnemy(GameObject newEnemy)
	{
		GameObject _parent = GetParent();
		EnemySplitter _parentEnemy = _parent.GetComponent<EnemySplitter> ();

		EnemySplitter newSplit = newEnemy.GetComponent<EnemySplitter> ();

		if (newSplit == null) {
			GameObject.DestroyImmediate (newEnemy);
			Debug.Log ("Error getting Enemy script");
			return;
		}


		newSplit.parentEnemy = _parentEnemy;
		newSplit.InitEnemy (this.m_editRect);
		
		_parentEnemy.childrenEnemies.Add (newEnemy.GetComponent<EnemySplitter> ());
	}

	public GameObject GetParent()
	{
		if (parentEnemy == null) {
			return this.gameObject;
		} else {
			return parentEnemy.GetParent ();
		}
	}

	public EnemySplitter GetParentSplitter()
	{
		return GetParent ().GetComponent<EnemySplitter> ();
	}

	public override void Kill()
	{
		if (parentEnemy == null) {
			m_isDestroyingChildren = childrenEnemies.Count > 0;

			if (!m_isDestroyingChildren)
				base.StartDying ();
		} else {
			if(!m_isDying)
				parentEnemy.Kill();
			base.StartDying ();
		}
	}

	public bool MaximumChildren()
	{
		return childrenEnemies.Count >= m_maxChildren;
	}

	public override void DestroyChildren()
	{
		if (Random.value < 0.75f)
			return;

		int randomDestoyed = Random.Range(0,childrenEnemies.Count-1);
		childrenEnemies [randomDestoyed].StartDying ();
		childrenEnemies.RemoveAt (randomDestoyed);

		if (childrenEnemies.Count == 0)
			Kill ();
	}
}
