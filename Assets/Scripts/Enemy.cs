using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityMovable {

	protected override void OnStart() {
		movementSpeed = 1.5f;
	}

	void Update () {
		if (!hasTarget) {
			if (Random.Range(0, 10) > 3) {
				GameObject sourceTile = GameController.Instance.FindClosest ("MilitaryHub", pos2d, null, 6);
				if (sourceTile != null) {
					hasTarget = true;
					targetObject = sourceTile;
				}
				else {
					GameObject economyTile = GameController.Instance.FindClosest ("EconomyHub", pos2d, null, 6);
					if (sourceTile != null) {
						hasTarget = true;
						targetObject = sourceTile;
					}
				}
			}
		}

		if (!hasTarget) {
			if (Random.Range(0, 5) > 2) {
				GameObject sourceTile = GameController.Instance.FindClosest ("Worker", pos2d, null, 12);
				if (sourceTile != null) {
					hasTarget = true;
					targetObject = sourceTile;
				}
			}
		}

		if (!hasTarget) {
			if (Random.Range(0, 4) > 1) {
				GameObject sourceTile = GameController.Instance.FindClosest ("Source", pos2d, null, 6);
				if (sourceTile != null) {
					hasTarget = true;
					targetObject = sourceTile;
				}
			}
		}

		if (!hasTarget) {
			if (Random.Range(0, 3) > 1) {
				GameObject sourceTile = GameController.Instance.FindClosest ("Grass", pos2d);
				if (sourceTile != null) {
					hasTarget = true;
					targetObject = sourceTile;
				}
			}
		}

		if (!hasTarget) {
			if (Random.Range(0, 4) > 1) {
				GameObject sourceTile = GameController.Instance.FindClosest ("MilitaryHub", pos2d);
				if (sourceTile != null) {
					hasTarget = true;
					targetObject = sourceTile;
				}
			}
		}

		if (!hasTarget) {
			GameObject sourceTile = GameController.Instance.FindClosest ("EconomyHub", pos2d);
			if (sourceTile != null) {
				hasTarget = true;
				targetObject = sourceTile;
			}
		}
	}

	protected override void OnReachedTarget(GameObject obj, int x, int y) {
		base.OnReachedTarget (obj, x, y);

		if (obj.CompareTag("Worker")) {
			Destroy (obj);
		}
		else if (obj.CompareTag("Grass") || obj.CompareTag("Source") || obj.CompareTag("EconomyHub") || obj.CompareTag("MilitaryHub")) {
			
			GameController.Instance.DestroyTile (x, y);
		}
	}
}
