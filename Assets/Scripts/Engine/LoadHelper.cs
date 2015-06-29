using UnityEngine;
using System.Collections;
using System;

public class LoadHelper : MonoBehaviour {

	GameController manager;
	AsyncOperation loadOperation = null;

	void Awake()
	{
		manager = this.GetComponent<GameController> ();

		Load ("MainGUI_001");
		AddLoad ("Player_001");

		AddLoad ("BackGround_Level_001");
		AddLoad ("Enemy_Pool_001");

	}
	void OnGUI()
	{
		if (loadOperation != null)
			GUI.Label(new Rect(0,0,100,50), (loadOperation.progress * 100f).ToString("0"));
	}

private IEnumerator LevelLoading()
	{
		yield return loadOperation;
//		do yield return new WaitForSeconds (0.1f); 
//	//while(Application.isLoadingLevel);
//		while (!loadOperation.isDone) ;
//	
//	Debug.Log (Application.loadedLevelName + " loaded");
//	yield break;
}

	public bool Load(string level)
{
		loadOperation = Application.LoadLevelAsync (level);
		StartCoroutine(LevelLoading());

		loadOperation = null;
		return level.Equals( Application.loadedLevelName );
	}
	
	public bool AddLoad(string level)
	{
		loadOperation = Application.LoadLevelAdditiveAsync (level);
		StartCoroutine(LevelLoading());
		
		loadOperation = null;
		return level.Equals( Application.loadedLevelName );
	}
	
}
