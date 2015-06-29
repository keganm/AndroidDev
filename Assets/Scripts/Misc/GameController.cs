using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameController : MonoBehaviour {

	public PlayerHelper playerHelper;
	public MainGUIHelper mainGuiHelper;
	public EnemySpawner enemySpawner;
	public GameScoring scoring;

	// Use this for initialization
	void Start () {
		
		if (mainGuiHelper == null) {
			GameObject mgui = GameObject.Find ("MainGUI");
			if(mgui == null){
				Debug.Log("Error finding main gui");
			}else{
				mainGuiHelper = mgui.GetComponent<MainGUIHelper> ();
			}
		}
	}

	public void Reset()
	{
		playerHelper.Reset ();
		enemySpawner.Reset ();
		scoring.Reset ();

		mainGuiHelper.HideGUI ();
	}

	public void PlayerDied()
	{
		enemySpawner.KillAll ();
	}

	public void StageCleared()
	{
		mainGuiHelper.ShowGUI();
	}
}
