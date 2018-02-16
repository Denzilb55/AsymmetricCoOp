using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : EntityMovable {

	public static List<GameObject> occupiedSources = new List<GameObject>();

	public int supplies;


	protected override void OnStart() {
		supplies = 0;
		movementSpeed = 1.32f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasTarget) {
			GameObject sourceTile = GameController.Instance.FindClosest ("Source", pos2d, occupiedSources);
			if (sourceTile != null) {
				hasTarget = true;
				targetObject = sourceTile;
				occupiedSources.Add (sourceTile);
			}
		}
	}

	protected override void OnReachedTarget(GameObject obj, int x, int y) {
		base.OnReachedTarget (obj, x, y);

		if (obj.CompareTag("Source")) {
			GameController.Instance.HarvestTile (x, y, this);

			targetObject = GameController.Instance.FindClosest ("EconomyHub", pos2d);
			hasTarget = true;
		}
		else if (obj.CompareTag("EconomyHub")) {
			GameController.Instance.supplies += supplies;
			supplies = 0;
		}
	}
}
