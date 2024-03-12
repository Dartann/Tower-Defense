using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] BaseTowerScript[] basePrefabs;
    [SerializeField] BaseTowerScript[] upgradePrefabs;

    private Dictionary<string, BaseTowerScript> baseTowers = new();
    private Dictionary<string, BaseTowerScript> UpgradedTower = new();


    private void Awake()
    {
        PlacementManager.createTower += InstantiateTower;

        foreach (BaseTowerScript tower in basePrefabs)
        {
            baseTowers.Add(tower.GetTowerData().towerID, tower);
        }
    }
    public Transform InstantiateTower(string towerID, GridObject currentGridTile)
    {
        var tower = baseTowers[towerID];

        if (currentGridTile.GetOnGridTowerTransform() == null)
            return Instantiate(tower, currentGridTile.transform.position, Quaternion.identity).transform;

        if(tower.GetTowerData().towerID == currentGridTile.GetOnGridTowerTransform().GetComponent<BaseTowerScript>().GetTowerData().towerID)
        {
            //var upgradedTower = baseTowers[towerID + 1];
            //return Instantiate(upgradedTower, currentGridTile.transform.position, Quaternion.identity).transform;
            return null;
        }
        return null;

    }

}
