using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyHub : MonoBehaviour {

	public Worker workerTemplate;

	public List<Worker> workers = new List<Worker>();
	public const int WORKER_COST = 6;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PurchaseWorker() {
		if (GameController.Instance.supplies >= WORKER_COST + 5) {
			GameController.Instance.supplies -= WORKER_COST;

			workers.Add(GameObject.Instantiate<Worker> (workerTemplate, transform.position, transform.rotation));
		}
	}
}
