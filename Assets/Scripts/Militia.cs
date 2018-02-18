using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Militia : EntityMovable {

	private MilitaryHub ownerHub;

	public void SetMilitaryHub(MilitaryHub hub) {
		ownerHub = hub;
	}

	protected override void OnStart() {
		movementSpeed = 4;
	}

	void Update () {
		if (!hasTarget) {
			targetObject = ownerHub.gameObject;
			hasTarget = true;
		}
	}

	protected override void OnReachedTarget(GameObject obj, int x, int y) {
		base.OnReachedTarget (obj, x, y);

		if (obj.CompareTag("Enemy")) {
			Destroy (obj);
			Destroy (gameObject);
			ownerHub.NotifyDestroyed (this);
		}
		else if (obj.CompareTag("MilitaryHub")) {
			Destroy (gameObject);
			ownerHub.ReturnToBase (this);
		}
	}

}
