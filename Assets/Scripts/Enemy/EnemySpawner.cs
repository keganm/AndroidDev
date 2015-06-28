using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour {
	[System.Serializable]
	public struct EnemyProbability
	{
		public GameObject enemy;
		public int probability;
	}

	public int maxEnemies = 10;
	private List<GameObject> enemies = new List<GameObject> ();
	public EnemyProbability[] enemyPrefabs;
	private int prefabWeightSum;

	public float enemySpawnTime = 2f; 
	private float enemySpawnTimer = 0f;

	public float spawnJitter = 0.95f;
	
	public EditRect m_editRect;
	public Rect m_spawnRect = new Rect (-1.0f, -1.0f, 2.0f, 2.0f);

	// Use this for initialization
	void Start () {
		m_editRect = this.GetComponent<EditRect> ();

		enemySpawnTimer = Time.time;
		prefabWeightSum = GetTotalProbability ();
		m_spawnRect = GetSpawnRect ();


		SpawnNewEnemy ();
	}

	public void Reset(){
		for (int i = 0; i < enemies.Count; ++i) {
			enemies[i].GetComponent<Enemy_Base>().Reset();
		}
		enemies.Clear ();

		enemySpawnTimer = 0f;
	}

	Rect GetSpawnRect()
	{
		Rect tmp;

		if (m_editRect != null) {

			tmp = new Rect(m_editRect.xMin,m_editRect.yMin,m_editRect.xMax,m_editRect.yMax);

		} else {
			tmp = new Rect(0,0,Screen.width,Screen.height);
		}

		return tmp;
	}

	public int GetEnemyCount()
	{
		return enemies.Count;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale <= 0f)
			return;

		CheckEnemyAlive ();

		if (Time.time - enemySpawnTimer > enemySpawnTime) {
			SpawnNewEnemy();
			enemySpawnTimer = Time.time;
		}
	}

	void SpawnNewEnemy()
	{
		if (enemies.Count >= maxEnemies || Random.value > spawnJitter)
			return;
		
		GetSpawnRect ();
		float _x = Random.Range (m_spawnRect.xMin,m_spawnRect.xMax);
		float _y = Random.Range (m_spawnRect.yMin,m_spawnRect.yMax);



		GameObject newEnemy = GameObject.Instantiate (GetEnemyWithWeight());
		newEnemy.transform.parent = this.transform;
		//Scaled to push z back down to 0
		Vector3 spawnPoint = new Vector3 (_x, _y, 0f);
		newEnemy.transform.localPosition = new Vector3(spawnPoint.x,spawnPoint.y, 0f);
	//	newEnemy.GetComponent<Enemy_Base> ().ContainerRect = spawnRectangle;
		newEnemy.GetComponent<Enemy_Base> ().InitEnemy (m_editRect);
		enemies.Add (newEnemy);
	}

	public bool AddNewEnemy(GameObject newEnemy)
	{
		if (enemies.Count >= maxEnemies) {
			GameObject.DestroyImmediate(newEnemy);
			return false;
		}

		newEnemy.transform.parent = this.transform;
		enemies.Add (newEnemy);
		return true;
	}

	void CheckEnemyAlive()
	{
		for(int i = enemies.Count - 1; i>=0; i--){
			if(enemies[i] == null)
			{
				enemies.Remove(enemies[i]);
			}else{

			Enemy_Base escript = enemies[i].GetComponent<Enemy_Base>();
			if(!escript.m_isAlive){
				escript.Kill();
				enemies.Remove(enemies[i]);
			}
			}
		}
	}

	private GameObject GetEnemyWithWeight()
	{
		int randomChoice = Random.Range (0, prefabWeightSum);

		int p = enemyPrefabs.Length - 1;
		while ((randomChoice -= enemyPrefabs[p].probability) > 0) {
			p--;
		}

		return enemyPrefabs [p].enemy;
	}

	private int GetTotalProbability()
	{
		int probailityRange = 0;
		foreach (EnemyProbability ep in enemyPrefabs) {
			probailityRange += ep.probability;
		}
		return probailityRange;
	}
}
