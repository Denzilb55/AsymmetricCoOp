using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovable : MonoBehaviour {

	protected bool hasTarget = false;
	protected Vector2 target {
		get {
			if (targetObject == null) {
				return posTarget;
			}
			else {
				return targetObject.transform.position;
			}
		}
	}

	protected Vector2 posTarget;
	protected GameObject targetObject;
	protected float movementSpeed;

	public Vector2 pos2d {
		get {
			return new Vector2 (transform.position.x, transform.position.y);
		}
	}

	public Vector2 pos2di {
		get {
			return new Vector2 ((int)transform.position.x, (int)transform.position.y);
		}
	}

	public void SetTarget(GameObject target) {
		hasTarget = true;
		targetObject = target;
	}

	// Use this for initialization
	void Start () {
		OnStart ();
	}

	protected virtual void OnStart() {
		
	}

	void FixedUpdate() {
		if (hasTarget) {

			if (targetObject == null) {
				hasTarget = false;
				targetObject = null;
				return;
			}

			Vector2 dir = target - pos2d;
			transform.position = pos2d + dir.normalized * Time.fixedDeltaTime * movementSpeed;
			transform.position = new Vector3 (transform.position.x, transform.position.y, -1);

			if (dir.sqrMagnitude < 0.05f * 0.05f) {
				OnReachedTarget (targetObject, (int)target.x, (int)target.y);
			}
		}
	}

	protected virtual void OnReachedTarget(GameObject obj, int x, int y) {
		hasTarget = false;
		targetObject = null;
	}
}
