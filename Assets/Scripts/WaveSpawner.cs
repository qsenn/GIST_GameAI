using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public static int EnemiesAlive = 0;

	public Wave[] waves;

	public Transform spawnPoint;

	public float timeBetweenWaves = 5f;
	private float countdown = 10f;

	public Text waveCountdownText;

	public GameManager gameManager;

	private int waveIndex = 0;

	void Update ()
	{
		if (EnemiesAlive > 0)
		{
			return;
		}

		if (waveIndex == waves.Length)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}

		if (countdown <= 0f)
		{
			Debug.Log("spawn");
			GenerateWave();
			StartCoroutine(SpawnWave_Adaptive());
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;

		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

		waveCountdownText.text = string.Format("{0:00.00}", countdown);
	}

	IEnumerator SpawnWave ()
	{
		PlayerStats.Rounds++;

		Wave wave = waves[waveIndex];

		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		waveIndex++;
	}

	IEnumerator SpawnWave_Adaptive ()
	{
		PlayerStats.Rounds++;
		Wave wave = GenerateWave();

		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		waveIndex++;
	}

	public Wave GenerateWave(){
		float pathLength = Waypoints.points.Length / 60f;
		Debug.Log(pathLength);
		int rand = Random.Range(0,3);
		Wave wave = new Wave(); 
		if (rand == 0){
			wave.enemy = Resources.Load<GameObject>("Enemies/Simple/Enemy_Simple");
			wave.count = (int)(pathLength * (System.Math.Round(3.0 * PlayerStats.TotalBudget / 100) - (PlayerStats.startLives - PlayerStats.Lives) * 2));

			wave.rate = 1 * (PlayerStats.TotalBudget) / 100;
		}
		else if (rand == 1){
			wave.enemy = Resources.Load<GameObject>("Enemies/Fast/Enemy_Fast");
			wave.count = (int)(pathLength * (System.Math.Round(4.5 * PlayerStats.TotalBudget / 100) - (PlayerStats.startLives - PlayerStats.Lives) * 3));
			wave.rate = 1 * (PlayerStats.TotalBudget) / 100;
		}
		else if (rand == 2){
			wave.enemy = Resources.Load<GameObject>("Enemies/Tough/Enemy_Tough");
			wave.count = (int)(pathLength * (System.Math.Round(1.5 * PlayerStats.TotalBudget / 100) - (PlayerStats.startLives - PlayerStats.Lives) * 1));
			wave.rate = 1 * (PlayerStats.TotalBudget) / 200;
		}
		
		return wave;
	}

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
