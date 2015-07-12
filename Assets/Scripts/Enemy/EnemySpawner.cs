using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour {
	public GameController m_gameController;
	private GameObject m_player;

	[System.Serializable]
	public struct EnemyProbability
	{
		public GameObject enemy;
		public int probability;
	}

	public int m_maxEnemies = 10;
	private List<GameObject> m_enemies = new List<GameObject> ();
	public EnemyProbability[] m_enemyPrefabs;
	private int m_prefabWeightSum;

	public float m_enemySpawnTime = 2f; 
	private float m_enemySpawnTimer = 0f;

	public float m_spawnJitter = 0.95f;
	
	public EditRect m_editRect;
	public Rect m_spawnRect = new Rect (-1.0f, -1.0f, 2.0f, 2.0f);

	public bool m_killingEnemies = false;

	// Use this for initialization
	void Start () {
		
		if (m_gameController == null) {
			GameObject gc = GameObject.FindWithTag ("GameController");
			if(gc == null)
				return;
			m_gameController = gc.GetComponent<GameController> ();
			if(m_gameController!= null)
				m_gameController.m_enemySpawner = this;
		}


		m_enemySpawnTimer = Time.time;
		m_prefabWeightSum = GetTotalProbability ();
		m_spawnRect = GetSpawnRect ();


		SpawnNewEnemy ();
	}

	public void GetBoundingRect()
	{
		m_editRect = GameObject.FindWithTag ("BoundingBox").GetComponent<EditRect> ();
	}

	public void Reset(){
		for (int i = 0; i < m_enemies.Count; ++i) {
			m_enemies[i].GetComponent<Enemy_Base>().Reset();
		}
		m_enemies.Clear ();

		m_enemySpawnTimer = 0f;
		m_killingEnemies = false;
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

	public void KillAll()
	{
		m_killingEnemies = true;
	}

	public bool AllEnemiesKilled()
	{
		return (m_killingEnemies && (m_enemies.Count == 0));
	}

	public void KillGenericEnemy (int enemy)
	{
		if (m_killingEnemies && m_enemies.Count > enemy) {
			if(m_enemies[enemy] == null)
				m_enemies.RemoveAt(enemy);
			else
				m_enemies [enemy].GetComponent<Enemy_Base> ().Kill ();
		}
	}

	public int GetEnemyCount()
	{
		return m_enemies.Count;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale <= 0f)
			return;

		if (m_killingEnemies) {
			KillGenericEnemy(0);
			KillGenericEnemy(1);
			if(AllEnemiesKilled())
				m_gameController.StageCleared();
		}

		CheckEnemyAlive ();

		if (Time.time - m_enemySpawnTimer > m_enemySpawnTime) {
			SpawnNewEnemy();
			m_enemySpawnTimer = Time.time;
		}
	}
	
	public void GetBoundingBox()
	{
		GameObject bb = GameObject.FindGameObjectWithTag ("BoundingBox");
		if (bb == null) {
			m_editRect = null;
			return;
		}
		m_editRect = bb.GetComponent<EditRect> ();
	}

	public bool GetPlayer()
	{
		if (m_player != null)
			return true;
		m_player = GameObject.FindGameObjectWithTag("Player");
		return (m_player != null);
	}

	void SpawnNewEnemy()
	{
		if (m_editRect == null) {
			GetBoundingBox();
			if(m_editRect == null)
				return;
			else
				m_spawnRect = GetSpawnRect();
		}

		if (m_enemies.Count >= m_maxEnemies || Random.value > m_spawnJitter || m_killingEnemies)
			return;
		
		GetSpawnRect ();
		if (!GetPlayer ()) {
			Debug.Log ("Error finding player tag");
			return;
		}


		Vector2 playerXY = new Vector2 (m_player.transform.position.x, m_player.transform.position.y);
		Vector2 spawnPosition = playerXY;
		while (Vector2.Distance(playerXY,spawnPosition) < 2f) {
			Debug.Log("Finding spawn position");
			spawnPosition = new Vector2(Random.Range (m_spawnRect.xMin,m_spawnRect.xMax),
			                            Random.Range (m_spawnRect.yMin,m_spawnRect.yMax));
		}

		//Scaled to push z back down to 0
		//Vector3 spawnPoint = new Vector3 (_x, _y, 0f);
		GameObject newEnemy = GameObject.Instantiate (GetEnemyWithWeight(),new Vector3(spawnPosition.x,spawnPosition.y, 0f),new Quaternion(0,0,0,0)) as GameObject;
		newEnemy.transform.parent = this.transform;
	//	newEnemy.GetComponent<Enemy_Base> ().ContainerRect = spawnRectangle;
		newEnemy.GetComponent<Enemy_Base> ().InitEnemy (m_editRect);
		m_enemies.Add (newEnemy);
	}

	public bool AddNewEnemy(GameObject newEnemy)
	{
		if (m_enemies.Count >= m_maxEnemies) {
			GameObject.DestroyImmediate(newEnemy);
			return false;
		}

		newEnemy.transform.parent = this.transform;
		m_enemies.Add (newEnemy);
		return true;
	}

	void CheckEnemyAlive()
	{
		for(int i = m_enemies.Count - 1; i>=0; i--){
			if(m_enemies[i] == null)
			{
				m_enemies.Remove(m_enemies[i]);
			}else{

			Enemy_Base _enemy = m_enemies[i].GetComponent<Enemy_Base>();
			if(!_enemy.IsAlive){
				_enemy.Kill();
				m_enemies.Remove(m_enemies[i]);
			}
			}
		}
	}

	private GameObject GetEnemyWithWeight()
	{
		int randomChoice = Random.Range (0, m_prefabWeightSum);

		int p = m_enemyPrefabs.Length - 1;
		while ((randomChoice -= m_enemyPrefabs[p].probability) > 0) {
			p--;
		}

		return m_enemyPrefabs [p].enemy;
	}

	private int GetTotalProbability()
	{
		int probailityRange = 0;
		foreach (EnemyProbability ep in m_enemyPrefabs) {
			probailityRange += ep.probability;
		}
		return probailityRange;
	}
}
