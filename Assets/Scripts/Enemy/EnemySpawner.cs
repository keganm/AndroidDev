using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour {
	public GameObject enemy01;

	public int maxEnemies = 10;
	private List<GameObject> enemies = new List<GameObject> ();

	public float enemySpawnTime = 2f; 
	private float enemySpawnTimer = 0f;


	// Use this for initialization
	void Start () {
		enemySpawnTimer = Time.time;
		SpawnNewEnemy ();
	}

	public int GetEnemyCount()
	{
		return enemies.Count;
	}
	
	// Update is called once per frame
	void Update () {
		CheckEnemyAlive ();

		if (Time.time - enemySpawnTimer > enemySpawnTime) {
			SpawnNewEnemy();
			enemySpawnTimer = Time.time;
		}
	}

	void SpawnNewEnemy()
	{
		if (enemies.Count >= maxEnemies)
			return;

		float _x = Random.Range (0f, (float)Screen.width);
		float _y = Random.Range (0f, (float)Screen.height);

		GameObject newEnemy = GameObject.Instantiate (enemy01);
		newEnemy.transform.parent = this.transform;
		newEnemy.transform.localPosition = Camera.main.ScreenToWorldPoint( new Vector3 (_x, _y, 0f) );
		enemies.Add (newEnemy);
	}

	void CheckEnemyAlive()
	{
		for(int i = enemies.Count - 1; i>=0; i--){
			Enemy escript = enemies[i].GetComponent<Enemy>();
			if(!escript.m_isAlive){
				escript.Kill();
				enemies.Remove(enemies[i]);
			}
		}
	}
}
