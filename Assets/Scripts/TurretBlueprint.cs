using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretBlueprint {

	public GameObject prefab;
	public int cost;

	public GameObject upgradedPrefab;
	public int upgradeCost;

	public string name;

	public int GetSellAmount ()
	{
		return cost / 2;
	}

}
