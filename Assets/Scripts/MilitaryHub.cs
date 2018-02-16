using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryHub : MonoBehaviour {

	public Soldier soldierTemplate;

	public List<Soldier> soldiers = new List<Soldier>();
	public const int SOLDIER_COST = 8;


	void SpawnSoldier() {
		soldiers.Add(GameObject.Instantiate (soldierTemplate, transform.position, Quaternion.identity));

	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnSoldier", 2, 8);
	}

	// Update is called once per frame
	void Update () {

	}
		
}
