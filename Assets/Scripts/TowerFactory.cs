using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] BaseTowerScript[] basePrefabs;
    [SerializeField] BaseTowerScript[] upgradePrefabs;

    private Dictionary<string, BaseTowerScript> baseTowers = new();
    private Dictionary<string, BaseTowerScript> AdvanceTowers = new();


    private void Awake()
    {
        PlacementManager.Event_requestNewTower += TowerFactory_InstantiateTower;
        PlacementManager.Event_isTowerUpgradeExist += TowerFactroy_IsTowerHasUpgradeVersion;


        foreach (BaseTowerScript tower in basePrefabs)
        {
            baseTowers.Add(tower.GetTowerData().towerID, tower);
        }

        foreach (BaseTowerScript tower in upgradePrefabs)
        {
            AdvanceTowers.Add(tower.GetTowerData().towerID, tower);
        }
    }
    private BaseTowerScript TowerFactory_InstantiateTower(string currentSelectedtowerID, GridObject currentSelectedGridObject)
    {
        var currentSelectedTower = baseTowers[currentSelectedtowerID];

        if (!currentSelectedGridObject.IsThereTowerOnGrid)
            return Instantiate(currentSelectedTower, currentSelectedGridObject.transform.position, Quaternion.identity);

        var UpgradedTower = GetTowerUpgradedVersion(currentSelectedGridObject, currentSelectedTower);

        if(!UpgradedTower)
            return null;

        return Instantiate(UpgradedTower, currentSelectedGridObject.transform.position, Quaternion.identity); ;

    }
    private bool TowerFactroy_IsTowerHasUpgradeVersion(string currentSelectedtowerID, GridObject currentSelectedGridObject)
    {
        var currentSelectedTower = baseTowers[currentSelectedtowerID];
        Debug.Log(GetTowerUpgradedVersion(currentSelectedGridObject, currentSelectedTower));
        return GetTowerUpgradedVersion(currentSelectedGridObject, currentSelectedTower);
       
    }

    // DÜZELT
    private BaseTowerScript GetTowerUpgradedVersion(GridObject currentSelectedGridObject, BaseTowerScript currentSelectedTower)
    {
        var gridTower = currentSelectedGridObject.GetTowerObjectOnGrid();

        if (!gridTower)
            return null;
           
        var gridTowerData = gridTower.GetTowerData();

        var UpradedVersionOfTower = gridTowerData.towerLinks.Where(x => x.linkedTower == currentSelectedTower.GetTowerData()).FirstOrDefault();

        if (UpradedVersionOfTower != null)
        {
            var UpgradedTower = AdvanceTowers[UpradedVersionOfTower.UpgradedTower.towerID];
            return UpgradedTower;
        }
        return null;
    }

}
