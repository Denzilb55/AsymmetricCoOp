﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : EntityMovable {

	private MilitaryHub ownerHub;

	public static List<GameObject> targetList = new List<GameObject> ();

	public void SetMilitaryHub(MilitaryHub hub) {
		ownerHub = hub;
	}

	protected override void OnStart() {
		movementSpeed = 2;
	}

	void Update () {

		if (!hasTarget || targetObject == ownerHub.gameObject) {
			GameObject t = GameController.Instance.FindClosest ("Enemy", pos2d, targetList);

			if (t != null) {
				SetTarget (t);
				targetList.Add (t);
			}
			else {
				SetTarget(ownerHub.gameObject);
			}
		}
	}

	protected override void OnReachedTarget(GameObject obj, int x, int y) {
		base.OnReachedTarget (obj, x, y);

		if (obj.CompareTag("Enemy")) {
			Destroy (obj);
		}
	}

}
