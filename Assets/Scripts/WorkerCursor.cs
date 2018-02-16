using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCursor : BaseCursor {

	public EconomyHub economyHub;

	public const int ECONOMY_HUB_COST = 15;

	bool readAxisV = true;
	bool readAxisH = true;

	float vTime = 0;
	float hTime = 0;
	protected override void HandleController() {

		if (readAxisV) {
			if (Input.GetAxis ("EconomyV") > 0.3f) {
				transform.position -= Vector3.up;
			} else if (Input.GetAxis ("EconomyV") < -0.3f) {
				transform.position += Vector3.up;
			}
			readAxisV = false;
			vTime = 0.3f;
		}
		else {
			vTime -= Time.deltaTime;
			if (vTime < 0) {
				if (Input.GetAxis ("EconomyV") > 0.3f) {
					transform.position -= Vector3.up;
				} else if (Input.GetAxis ("EconomyV") < -0.3f) {
					transform.position += Vector3.up;
				}
				vTime = 0.18f;
			}
		}

		if (Mathf.Abs(Input.GetAxis ("EconomyV")) <= 0.3f) {
			readAxisV = true;
			vTime = 0;
		}

		if (readAxisH) {
			if (Input.GetAxis ("EconomyH") > 0.3f) {
				transform.position -= Vector3.left;
			} else if (Input.GetAxis ("EconomyH") < -0.3f) {
				transform.position += Vector3.left;
			}
			readAxisH = false;
			hTime = 0.3f;
		}
		else {
			hTime -= Time.deltaTime;
			if (hTime < 0) {
				if (Input.GetAxis ("EconomyH") > 0.3f) {
					transform.position -= Vector3.left;
				} else if (Input.GetAxis ("EconomyH") < -0.3f) {
					transform.position += Vector3.left;
				}
				hTime = 0.18f;
			}
		}

		if (Mathf.Abs(Input.GetAxis ("EconomyH")) <= 0.3f) {
			readAxisH = true;
			hTime = 0;
		}

		if (Input.GetButtonDown("EconomyA")) {
			DoAction ();
		}

		if (Input.GetButtonDown("EconomyB")) {
			DoAction2 ();
		}
	}



	protected override void DoAction () {
		if (GameController.Instance.CheckTileTag (x, y, "Source")) {
			GameController.Instance.HarvestTile (x, y);
		} else if (GameController.Instance.CheckTileTag (x, y, "EconomyHub")) {
			GameController.Instance.GetTile<EconomyHub> (x, y).PurchaseWorker();
		}
	}

	protected override void DoAction2 () {
		if (GameController.Instance.supplies >= ECONOMY_HUB_COST + 5 && GameController.Instance.CheckTileTag(x, y, "Grass")) {
			EconomyHub economyHubObj = GameObject.Instantiate (economyHub, transform.position, transform.rotation);
			GameController.Instance.ReplaceTile (x, y, economyHubObj.gameObject);
			GameController.Instance.supplies -= ECONOMY_HUB_COST;
		}
	}

}