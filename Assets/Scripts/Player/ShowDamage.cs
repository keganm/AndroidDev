using UnityEngine;
using System.Collections;

public class ShowDamage : MonoBehaviour {
	protected bool m_isShowingDamage = false;

	[SerializeField]		protected int m_showTimer = 100;
	protected int m_showTime = 0;
	[SerializeField]		protected int m_showDefaultDelta = 1;
	protected int m_showDelta = 1;
	
	[SerializeField]		protected Vector2 m_defaultUVDelta = new Vector2(0f,0.1f);
	protected Vector2 m_UVDelta;
	protected Vector2 m_UV = new Vector2(0f,0f);

	[SerializeField]		protected MeshRenderer m_damageMeshRenderer;

	public bool testStart = false;
	private bool lastTestStart = false;
	// Use this for initialization
	void Start () {
		m_showTime = m_showTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_showTime > m_showTimer) {
			m_damageMeshRenderer.enabled = false;
			if(testStart)
				StartDamage();
			return;
		}

		m_damageMeshRenderer.enabled = true;
		m_UV += m_UVDelta;
		m_damageMeshRenderer.material.SetTextureOffset("_MainTex", m_UV);

		m_showTime += m_showDelta;

	}

	public void StartDamage()
	{
		StartDamage (m_showDefaultDelta, m_defaultUVDelta);
	}

	public void StartDamage(int _showDelta)
	{
		StartDamage (_showDelta, m_defaultUVDelta);
	}

	public void StartDamage(Vector2 _uvDelta)
	{
		StartDamage (m_showDefaultDelta, _uvDelta);
	}

	public void StartDamage(int _showDelta, Vector2 _uvDelta)
	{
		m_showDelta = _showDelta;
		m_showTime = 0;
		m_UVDelta = _uvDelta;
	}
}
