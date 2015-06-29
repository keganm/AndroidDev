using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MainGUIHelper : MonoBehaviour {
	public GameController gameController;
	public Canvas[] canvases;
	public bool showGUI = false;
	private bool m_isHidden = false;

	// Use this for initialization
	void OnEnable () {
		
		if (gameController == null) {
			gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
			if(gameController!= null)
				gameController.mainGuiHelper = this;
		}
		canvases = this.GetComponentsInChildren<Canvas>();

		HideGUI ();
	}

	void Update()
	{
		if (showGUI && m_isHidden)
			ShowGUI ();

		if (!showGUI && !m_isHidden)
			HideGUI ();

		SetPause ();
	}

	void OnGUI() {
		SetPause ();
	}

	public void SetPause()
	{
		float timeScale = 1.0f;

		if (!m_isHidden)
			timeScale = 0f;

		Time.timeScale = timeScale;
	}

	public void ShowGUI()
	{
		foreach (Canvas c in canvases) {
			c.enabled = true;
		}
		m_isHidden = false;
		showGUI = true;
	}
	
	public void HideGUI()
	{
		foreach (Canvas c in canvases) {
			c.enabled = false;
		}
		m_isHidden = true;
		showGUI = false;
	}

	public void Reset()
	{
		if (gameController != null)
			gameController.Reset ();
	}
}
