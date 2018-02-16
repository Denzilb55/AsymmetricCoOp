using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : EntityMovable {

	protected override void OnStart() {
		movementSpeed = 2;
	}

	void Update () {
		if (!hasTarget) {
			GameObject sourceTile = GameController.Instance.FindClosest ("Enemy", pos2d);
			if (sourceTile != null) {
				hasTarget = true;
				targetObject = sourceTile;
			}
		}
	}

	protected override void OnReachedTarget(GameObject obj, int x, int y) {
		base.OnReachedTarget (obj, x, y);

		if (obj.CompareTag("Enemy")) {
			Destroy (obj);
			Destroy (gameObject);
		}
	}
}
