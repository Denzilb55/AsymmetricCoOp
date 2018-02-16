using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryCursor : BaseCursor {
	public MilitaryHub militaryHub;

	public const int HUB_COST = 20;

	public Vector2 pos2di {
		get {
			return new Vector2 ((int)transform.position.x, (int)transform.position.y);
		}
	}

	bool readAxisV = true;
	bool readAxisH = true;

	float vTime = 0;
	float hTime = 0;
	protected override void HandleController() {

		if (readAxisV) {
			if (Input.GetAxis ("MilitaryV") > 0.3f) {
				transform.position -= Vector3.up;
			} else if (Input.GetAxis ("MilitaryV") < -0.3f) {
				transform.position += Vector3.up;
			}
			readAxisV = false;
			vTime = 0.25f;
		}
		else {
			vTime -= Time.deltaTime;
			if (vTime < 0) {
				if (Input.GetAxis ("MilitaryV") > 0.3f) {
					transform.position -= Vector3.up;
				} else if (Input.GetAxis ("MilitaryV") < -0.3f) {
					transform.position += Vector3.up;
				}
				vTime = 0.12f;
			}
		}

		if (Mathf.Abs(Input.GetAxis ("MilitaryV")) <= 0.3f) {
			readAxisV = true;
			vTime = 0;
		}

		if (readAxisH) {
			if (Input.GetAxis ("MilitaryH") > 0.3f) {
				transform.position -= Vector3.left;
			} else if (Input.GetAxis ("MilitaryH") < -0.3f) {
				transform.position += Vector3.left;
			}
			readAxisH = false;
			hTime = 0.25f;
		}
		else {
			hTime -= Time.deltaTime;
			if (hTime < 0) {
				if (Input.GetAxis ("MilitaryH") > 0.3f) {
					transform.position -= Vector3.left;
				} else if (Input.GetAxis ("MilitaryH") < -0.3f) {
					transform.position += Vector3.left;
				}
				hTime = 0.12f;
			}
		}

		if (Mathf.Abs(Input.GetAxis ("MilitaryH")) <= 0.3f) {
			readAxisH = true;
			hTime = 0;
		}

		if (Input.GetButtonDown("MilitaryA")) {
			DoAction ();
		}

		if (Input.GetButtonDown("MilitaryB")) {
			DoAction2 ();
		}
	}

	protected override void DoAction () {
		if (GameController.Instance.CheckTileTag (x, y, "Corruption")) {
			GameController.Instance.RestoreTile (x, y);
		}

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		foreach (GameObject e in enemies) {
			Vector2 ePos = e.GetComponent<Enemy> ().pos2di;
			if (ePos == pos2di || 
				ePos == pos2di + Vector2.down ||
				ePos == pos2di + Vector2.up ||
				ePos == pos2di + Vector2.left ||
				ePos == pos2di + Vector2.right
			) {
				Destroy (e);
			}
		}
			
	}

	protected override void DoAction2 () {
		if (GameController.Instance.supplies >= HUB_COST + 5 && GameController.Instance.CheckTileTag(x, y, "Grass")) {
			MilitaryHub economyHubObj = GameObject.Instantiate (militaryHub, transform.position, transform.rotation);
			GameController.Instance.ReplaceTile (x, y, economyHubObj.gameObject);
			GameController.Instance.supplies -= HUB_COST;
		}
	}
}
