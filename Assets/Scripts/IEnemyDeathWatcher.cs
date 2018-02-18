using System;
using UnityEngine;

public interface IEnemyDeathWatcher
{
	void NotifyDestroyed(GameObject enemy);
}
