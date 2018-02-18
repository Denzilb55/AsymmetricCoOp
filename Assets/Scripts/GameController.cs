using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject grassTile;
	public GameObject sourceTile;
	public GameObject corruptionTile;
	public GameObject dirtTile;

	public GameObject enemyEntity;
	public GameObject workerEntity;
	public GameObject soldierEntity;

	public GameObject camera;
	public Text suppliesText;

	public const int MAP_WIDTH = 45;
	public const int MAP_HEIGHT = 27;

	public GameObject[,] tiles = new GameObject[MAP_WIDTH,MAP_HEIGHT];

	public static GameController Instance;

	public int supplies;

	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		GenerateMap ();

		camera.transform.position = new Vector3 (MAP_WIDTH / 2.0f, MAP_HEIGHT / 2.0f, -10);
		InvokeRepeating ("SpawnRandomSource", 2, 2);
		Invoke("SpawnEnemy", 2.4f);
		InvokeRepeating ("CorruptionPenalties", 15, 5);

		supplies = 40;

	}

	float enemiesToSpawn = 0;
	float difficulty = 0.22f;
	
	// Update is called once per frame
	void Update () {
		suppliesText.text = "Supplies: " + supplies;

		if (supplies <= 0) {
			suppliesText.text = "LOSE";
			supplies = -10000;
		}

		if (supplies >= 1000) {
			suppliesText.text = "WIN";
			supplies = 10000;
		}

		if (Input.GetKeyDown(KeyCode.L)) {
			Application.LoadLevel (Application.loadedLevel);
		}

		enemiesToSpawn += Time.deltaTime * difficulty;
		difficulty += Time.deltaTime * 0.0042f;

		for (int i = 0; i < (int)enemiesToSpawn; i++) {
			Vector2 r = Random.insideUnitCircle.normalized * (MAP_WIDTH / 2 + 5) + new Vector2 (MAP_WIDTH / 2.0f, MAP_HEIGHT / 2.0f);
			GameObject.Instantiate (enemyEntity, r, Quaternion.identity);
		}

		enemiesToSpawn -= (int)enemiesToSpawn;

	}

	void CorruptionPenalties() {
		supplies -= (int)(GameObject.FindGameObjectsWithTag ("Corruption").Length * 0.25f);
	}

	void SpawnRandomSource() {
		for (int i = 0; i < GameObject.FindGameObjectsWithTag ("EconomyHub").Length +1; i++) {
			int x = Random.Range (0, MAP_WIDTH);
			int y = Random.Range (0, MAP_HEIGHT);

			if (tiles[x, y].CompareTag("Grass")) {
				ReplaceTile (x, y, GameObject.Instantiate (sourceTile));
			}
		}

	}

	float d = 0;

	void SpawnEnemy() {
		/*Vector2 r = Random.insideUnitCircle.normalized * (MAP_WIDTH / 2 + 2) + new Vector2 (MAP_WIDTH / 2.0f, MAP_HEIGHT / 2.0f);

		GameObject.Instantiate (enemyEntity, r, Quaternion.identity);

		d += 0.02f;

		float h = 5 - d;

		if (h < 0.02f) {
			h = 0.02f;
		}

		Invoke("SpawnEnemy", h);*/
	}

	private void GenerateMap() {
		for (int i = 0; i < MAP_WIDTH; i ++) {
			for (int j = 0; j < MAP_HEIGHT; j ++) {
				
				if (Random.Range(0, 20) == 0) {
					tiles [i, j] = GameObject.Instantiate (sourceTile);
					tiles [i, j].transform.position = new Vector2 (i, j);
				}
				else if (Random.Range(0, 100) == 0) {
					tiles [i, j] = GameObject.Instantiate (corruptionTile);
					tiles [i, j].transform.position = new Vector2 (i, j);
				}
				else {
					tiles [i, j] = GameObject.Instantiate (grassTile);
					tiles [i, j].transform.position = new Vector2 (i, j);	
				}

			}
		}
	}

	public bool IsInWorldBounds(int x, int y) {
		return x >= 0 && y >= 0 && y < MAP_HEIGHT && x < MAP_WIDTH;
	}

	public bool CheckTileTag(int x, int y, string tag) {
		return tiles [x, y].CompareTag (tag);
	}

	public void HarvestTile(int x, int y) {
		if (CheckTileTag(x, y, "Source")) {
			Worker.occupiedSources.Remove (tiles [x, y]);
			Destroy(tiles [x, y]);
			tiles [x, y] = GameObject.Instantiate (grassTile);
			tiles [x, y].transform.position = new Vector2 (x, y);

			supplies += 2;
		}
	}

	public void HarvestTile(int x, int y, Worker harvestBy) {
		if (CheckTileTag(x, y, "Source")) {
			Worker.occupiedSources.Remove (tiles [x, y]);
			Destroy(tiles [x, y]);
			tiles [x, y] = GameObject.Instantiate (grassTile);
			tiles [x, y].transform.position = new Vector2 (x, y);

			harvestBy.supplies += 5;
		}
	}

	public void RestoreTile(int x, int y) {
		if (supplies < 8) {
			return;
		}
		if (CheckTileTag(x, y, "Corruption")) {
			Worker.occupiedSources.Remove (tiles [x, y]);
			Destroy(tiles [x, y]);
			tiles [x, y] = GameObject.Instantiate (dirtTile);
			tiles [x, y].transform.position = new Vector2 (x, y);

			supplies -= 1;
			return;
		}

		if (CheckTileTag(x, y, "Dirt")) {
			Worker.occupiedSources.Remove (tiles [x, y]);
			Destroy(tiles [x, y]);
			tiles [x, y] = GameObject.Instantiate (grassTile);
			tiles [x, y].transform.position = new Vector2 (x, y);

			supplies -= 1;
			return;
		}
	}

	public void DestroyTile(int x, int y) {
		Worker.occupiedSources.Remove (tiles [x, y]);
		Destroy(tiles [x, y]);
		tiles [x, y] = GameObject.Instantiate (corruptionTile);
		tiles [x, y].transform.position = new Vector2 (x, y);
	}

	public void ReplaceTile(int x, int y, GameObject newTile) {
		Destroy(tiles [x, y]);
		tiles [x, y] = newTile;
		tiles [x, y].transform.position = new Vector2 (x, y);
	}

	public T GetTile<T>(int x, int y) {
		return tiles [x, y].GetComponent<T>();
	}

	public GameObject FindClosest(string tag, Vector2 pos, List<GameObject> excludeList = null, int maxRange = 10000) {
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

		float sqrD = 1000000;
		GameObject closest = null;

		foreach (GameObject obj in objectsWithTag) {
			if (excludeList != null && excludeList.Contains(obj)) {
				continue;
			}

			Vector2 d = new Vector2 (obj.transform.position.x, obj.transform.position.y) - pos;
			float newSqrD = d.sqrMagnitude;

			if (newSqrD < sqrD && newSqrD < maxRange * maxRange) {
				sqrD = newSqrD;
				closest = obj;

				if (sqrD <= 1) {
					return closest;
				}
			}
		}

		return closest;
	}

	public GameObject FindClosestAvailableWorker(Vector2 pos) {
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Worker");

		float sqrD = 1000000;
		GameObject closest = null;

		foreach (GameObject obj in objectsWithTag) {
			Vector2 d = new Vector2 (obj.transform.position.x, obj.transform.position.y) - pos;
			float newSqrD = d.sqrMagnitude;

			if (newSqrD < sqrD && obj.GetComponent<Worker>().IsAvailableForRecruitment()) {
				sqrD = newSqrD;
				closest = obj;

				if (sqrD <= 1) {
					return closest;
				}
			}
		}

		return closest;
	}
		
}
