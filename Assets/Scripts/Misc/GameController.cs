using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameController : MonoBehaviour {

	[SerializeField]	public PlayerHelper m_playerHelper;
	[SerializeField]	public MainGUIHelper m_mainGuiHelper;
	[SerializeField]	public EnemySpawner m_enemySpawner;
	[SerializeField]	public GameScoring m_scoring;

	// Use this for initialization
	void Start () {
		
		if (m_mainGuiHelper == null) {
			GameObject mgui = GameObject.Find ("MainGUI");
			if(mgui == null){
				Debug.Log("Error finding main gui");
			}else{
				m_mainGuiHelper = mgui.GetComponent<MainGUIHelper> ();
			}
		}

		if (m_scoring == null) {
			m_scoring = this.GetComponent<GameScoring>();
		}
	}

	public void Reset()
	{
		if(m_playerHelper != null)
			m_playerHelper.Reset ();
		if(m_enemySpawner != null)
			m_enemySpawner.Reset ();
		if (m_scoring != null)
			m_scoring.Reset ();
		if (m_mainGuiHelper != null)
			m_mainGuiHelper.HideGUI ();
	}

	public void PlayerDied()
	{
		m_enemySpawner.KillAll ();
	}

	public void StageCleared()
	{
		m_mainGuiHelper.ShowGUI();
	}
}
