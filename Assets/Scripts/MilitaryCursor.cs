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
