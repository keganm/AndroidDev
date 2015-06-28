using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

	public PlayerHelper playerHelper;

	public Text lifeBarText;
	public Text lifeText;
	public int lifeTextLength = 16;
	public float beginLife = 1000f;
	public float life = 1000f;

	public bool m_isDying = false;
	public bool m_isDead = false;

	void Start()
	{
		if (playerHelper == null)
			playerHelper = this.GetComponent<PlayerHelper> ();
		lifeText = GameObject.Find ("LifeText").GetComponent<Text> ();
		lifeBarText = GameObject.Find ("LifeBar").GetComponent<Text> ();
		OutputToGUI ();
	}
	
	public void Reset()
	{
		this.GetComponentInChildren<MeshRenderer> ().enabled = true;
		m_isDying = false;
		m_isDead = false;

		life = beginLife;
		OutputToGUI ();
	}

	void Update()
	{
		ParticleSystem psys = this.GetComponentInChildren<ParticleSystem> ();
		if (m_isDying && psys.particleCount < 10) {
			m_isDead = true;
			if(playerHelper != null)
				playerHelper.PlayerDied();
		}
	}

	public bool TakeDamage(float damage)
	{
		life -= damage;

		if (life < 0) {
			if(!m_isDying)
				StartDying();
			life = 0;
		}

		OutputToGUI ();
		return m_isDying;
	}

	public void StartDying()
	{
		m_isDying = true;
		this.GetComponentInChildren<MeshRenderer> ().enabled = false;
		this.GetComponentInChildren<ParticleSystem> ().Emit (5000);
	}

	public void OutputToGUI()
	{
		string outString = "";
		char outChar = '.';
		int outLife = 8;
		if (m_isDead) {
			outChar = 'x';
		} else {
			outLife = Mathf.FloorToInt( (life / beginLife) * (float)lifeTextLength );
		}

		for (int i = 0; i < outLife; i++) {
			outString += outChar;
		}
		lifeBarText.text = outString;
		lifeText.text = life.ToString ("0000");
	}
}
