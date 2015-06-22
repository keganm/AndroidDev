using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scoring : MonoBehaviour {
	public Text scoreText;
	public EnemySpawner enemySpawner;
	public int score = 0;

	
	public float scoreTime = 1f; 
	private float scoreTimer = 0f;

	// Use this for initialization
	void Start () {
		enemySpawner = GameObject.FindObjectOfType<EnemySpawner> ();
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();
	}

	public void Reset()
	{
		score = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Time.time - scoreTimer > scoreTime) {
			score += enemySpawner.GetEnemyCount ();
			scoreText.text = score.ToString ();

			scoreTimer = Time.time;
		}
	}
}
