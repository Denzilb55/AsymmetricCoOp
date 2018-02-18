using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryHub : MonoBehaviour, IEnemyDeathWatcher {

	public Militia militiaTemplate;
	public Soldier soldierTemplate;

	public Soldier assignedSoldier;
	public Worker assignedWorker;

	public List<Militia> militias = new List<Militia>();
	public const int SOLDIER_COST = 8;

	private int readyMilitias = 0;

	private List<GameObject> currentTargetList = new List<GameObject> ();

	public Vector2 pos2d {
		get {
			return new Vector2 (transform.position.x, transform.position.y);
		}
	}


	void ReadyMilitia() {
		readyMilitias++;

		if (readyMilitias + militias.Count >= 3) {
			CancelInvoke ("ReadyMilitia");
		}
	}

	Militia SpawnMilitia() {
		if (readyMilitias > 0) {
			Militia s = GameObject.Instantiate (militiaTemplate, transform.position, Quaternion.identity);
			militias.Add (s);
			s.SetMilitaryHub (this);
			readyMilitias--;

			return s;
		}

		return null;
	}

	void OnDestroy() {
		if (assignedSoldier != null) {
			Destroy (assignedSoldier.gameObject);
		}

		if (assignedWorker != null) {
			assignedWorker.GetComponent<Worker> ().SetTarget (null);
		}

		foreach (Militia m in militias) {
			Destroy (m.gameObject);
		}
	}

	public void NotifyDestroyed(Militia militia) {
		militias.Remove (militia);

		if (readyMilitias + militias.Count < 3) {
			InvokeRepeating ("ReadyMilitia", 25, 25);
		}
	}

	public void NotifyDestroyed(GameObject enemy) {
		currentTargetList.Remove (enemy);
	}

	public void ReturnToBase(Militia militia) {
		militias.Remove (militia);
		readyMilitias++;
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("ReadyMilitia", 2, 25);
	}

	// Update is called once per frame
	void Update () {

		while (readyMilitias > 0) {
			GameObject t = GameController.Instance.FindClosest ("Enemy", pos2d, currentTargetList, 4);

			if (t == null) {
				break;
			}

			currentTargetList.Add (t);

			Militia m = SpawnMilitia ();
			m.SetTarget (t);
		}

		if (assignedSoldier == null && assignedWorker == null) {
			GameObject t = GameController.Instance.FindClosestAvailableWorker (pos2d);

			if (t !=  null) {
				assignedWorker = t.GetComponent<Worker> ();
				assignedWorker.SetTarget (gameObject);
			}
		}

	}

	public void Recruit(Worker worker) {
		Destroy (worker.gameObject);

		assignedSoldier = GameObject.Instantiate (soldierTemplate, transform.position, Quaternion.identity);
		assignedSoldier.SetMilitaryHub (this);
		assignedWorker = null;
	}
		
}
