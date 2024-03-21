using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TowerDataSO;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] BaseTowerScript[] basePrefabs;
    [SerializeField] BaseTowerScript[] upgradePrefabs;

    private Dictionary<string, BaseTowerScript> simpleTowers = new();
    private Dictionary<string, BaseTowerScript> AdvanceTowers = new();

    public string CurrentSelectedTowerID { get; private set; }
    public BaseTowerScript CurrentSelectedTower { get; private set; }
    public BaseTowerScript CurrentTowerUpgradedVersion { get; private set; }

    private void Awake()
    {    
        Shop.Event_UpdateCurrentBuyedTowerID += TowerFactory_UpdateCurrentSelectedTowerID;

        GridObject.Event_CheckTowerHaveUpgrade += TowerFactory_IsTowerUpgradeExist;

        SetTowerPrefabs();

    }
    private void SetTowerPrefabs()
    {
        foreach (BaseTowerScript tower in basePrefabs)
        {
            simpleTowers.Add(tower.GetTowerData().towerID, tower);
        }

        foreach (BaseTowerScript tower in upgradePrefabs)
        {
            AdvanceTowers.Add(tower.GetTowerData().towerID, tower);
        }
    }
    private void TowerFactory_UpdateCurrentSelectedTowerID(BaseTowerScript currentSelectedTowerScript)
    {
        CurrentSelectedTowerID = currentSelectedTowerScript.GetTowerData().towerID;
        CurrentSelectedTower = simpleTowers[CurrentSelectedTowerID];
    }

    public BaseTowerScript InstantiateTower(GridObject currentGridObject)
    {
        if (!currentGridObject.IsThereTowerOnGrid)
            return CurrentSelectedTower;

        if (CurrentTowerUpgradedVersion != null)
            return CurrentTowerUpgradedVersion;

        return null;
    }
    private BaseTowerScript GetTowerUpgrade(BaseTowerScript towerOnGrid)
    {
        var gridTowerData = towerOnGrid.GetTowerData();

        var UpgradedVersionOfTower = gridTowerData.towerLinks.Where(x => x.linkedTower == CurrentSelectedTower.GetTowerData())
            .FirstOrDefault();
       
        if (UpgradedVersionOfTower != null)
            CurrentTowerUpgradedVersion = AdvanceTowers[UpgradedVersionOfTower.UpgradedTower.towerID];
        else
            CurrentTowerUpgradedVersion = null;

        return CurrentTowerUpgradedVersion;
    }
    private bool TowerFactory_IsTowerUpgradeExist(BaseTowerScript towerOnGrid) => GetTowerUpgrade(towerOnGrid);


}
