using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySplitter : Enemy_Base {
	[SerializeField]	protected int m_retargetTimer = 50;
	[SerializeField]	protected int m_splitTimer = 100;
	[SerializeField]	protected float m_targetRadius = 0.5f;

	protected int m_retargetTime = 50;
	protected int m_splitTime = 0;
	
	protected Vector3 m_targetArea;
	protected Vector3 m_nonTarget = Vector3.zero;

	protected EnemySplitter m_parentEnemy = null;
	protected List<EnemySplitter> m_childrenEnemies = new List<EnemySplitter>();

	
	override public void InitEnemy(EditRect _rect)
	{
		base.InitEnemy (_rect);

		ReTarget ();

		m_targetArea = RandContainerPosition ();
	}

	override public void Reset()
	{
		for (int i = 0; i < m_childrenEnemies.Count; ++i) {
			m_childrenEnemies[i].Reset();
		}
		m_childrenEnemies.Clear ();
		base.Reset ();
	}
	
	override public void UpdateMovement()
	{
		ReTarget ();
		SplitEnemy ();

		m_acceleration = (m_nonTarget - m_position) * m_accelerationScale;
		
		base.ApplyMovement (m_acceleration);


	}

	private void ReTarget()
	{

		m_retargetTime++;

		if (m_retargetTime < m_retargetTimer)
			return;

		m_retargetTime = 0;

		if (m_parentEnemy == null) {


			Vector3 nta = RandContainerPosition();

			m_targetArea = Vector3.Lerp(m_targetArea, nta, 0.1f);
		}

		EnemySplitter _parent = GetParentSplitter ();
		float radius = ((float)_parent.m_childrenEnemies.Count / (float)m_maxChildren) * m_targetRadius;
		Vector2 area = Random.insideUnitCircle * radius;
		Vector2 size = ContainerRect.size;
		area.Scale (Camera.main.ScreenToWorldPoint( size ));

		Vector3 centerRandom = GetParent().GetComponent<EnemySplitter>().m_targetArea;
		Vector3 newTarget = new Vector3 (centerRandom.x + area.x, centerRandom.y + area.y, 0f);

		//newTarget = RandContainerWorldPosition ();
		m_nonTarget = Vector3.Lerp(m_nonTarget, newTarget, 0.5f);
	}

	private void SplitEnemy()
	{
		m_splitTime++;

		if (m_splitTime < m_splitTimer || kSPAWNER == null)
			return;
		
		m_splitTime = 0;

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


		newSplit.m_parentEnemy = _parentEnemy;
		newSplit.InitEnemy (this.m_editRect);
		
		_parentEnemy.m_childrenEnemies.Add (newEnemy.GetComponent<EnemySplitter> ());
	}

	public GameObject GetParent()
	{
		if (m_parentEnemy == null) {
			return this.gameObject;
		} else {
			return m_parentEnemy.GetParent ();
		}
	}

	public EnemySplitter GetParentSplitter()
	{
		return GetParent ().GetComponent<EnemySplitter> ();
	}

	public override void Kill()
	{
		if (m_parentEnemy == null) {
			m_isDestroyingChildren = m_childrenEnemies.Count > 0;

			if (!m_isDestroyingChildren)
				base.StartDying ();
		} else {
			if(!m_isDying)
				m_parentEnemy.Kill();
			base.StartDying ();
		}
	}

	public bool MaximumChildren()
	{
		return m_childrenEnemies.Count >= m_maxChildren;
	}

	public override void DestroyChildren()
	{
		if (Random.value < 0.75f)
			return;

		int randomDestoyed = Random.Range(0,m_childrenEnemies.Count-1);
		m_childrenEnemies [randomDestoyed].StartDying ();
		m_childrenEnemies.RemoveAt (randomDestoyed);

		if (m_childrenEnemies.Count == 0)
			Kill ();
	}
}
