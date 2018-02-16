using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCursor : MonoBehaviour {

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;

	public KeyCode action;
	public KeyCode action2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(up)) {
			transform.position += Vector3.up;
		}

		if (Input.GetKeyDown(down)) {
			transform.position += Vector3.down;
		}

		if (Input.GetKeyDown(left)) {
			transform.position += Vector3.left;
		}

		if (Input.GetKeyDown(right)) {
			transform.position += Vector3.right;
		}

		if (Input.GetKeyDown(action)) {
			DoAction ();
		} 

		if (Input.GetKeyDown(action2)) {
			DoAction2 ();
		} 

		HandleController ();

		float x = transform.position.x;
		float y = transform.position.y;

		if (x < 0) {
			x = GameController.MAP_WIDTH - 1;
		}

		if (x >= GameController.MAP_WIDTH) {
			x = 0;
		}

		if (y < 0) {
			y = GameController.MAP_HEIGHT - 1;
		}

		if (y >= GameController.MAP_HEIGHT) {
			y = 0;
		}

		transform.position = new Vector3 (x, y, -5);
	}

	protected virtual void HandleController() {
		
	}

	protected abstract void DoAction ();

	protected abstract void DoAction2 ();

	public int x {
		get {
			return (int)transform.position.x;
		}
	}

	public int y {
		get {
			return (int)transform.position.y;
		}
	}
}
