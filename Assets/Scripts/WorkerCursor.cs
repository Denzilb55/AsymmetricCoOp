using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCursor : BaseCursor {

	public EconomyHub economyHub;

	public const int ECONOMY_HUB_COST = 15;


	protected override void DoAction () {
		if (GameController.Instance.CheckTileTag (x, y, "Source")) {
			GameController.Instance.HarvestTile (x, y);
		} else if (GameController.Instance.CheckTileTag (x, y, "EconomyHub")) {
			GameController.Instance.GetTile<EconomyHub> (x, y).PurchaseWorker();
		} else if (GameController.Instance.CheckTileTag (x, y, "Dirt")) {
			GameController.Instance.RestoreTile (x, y);
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