using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    public static Action<BaseTowerScript> Event_UpdateCurrentBuyedTower;
    public static Action<BaseTowerScript> Event_CurrentGridTowerUpgradeVersion;

    [SerializeField] BaseTowerScript[] basePrefabs;
    [SerializeField] BaseTowerScript[] upgradePrefabs;

    private Dictionary<string, BaseTowerScript> simpleTowers = new();
    private Dictionary<string, BaseTowerScript> AdvanceTowers = new();

    string BuyedTowerID;
    BaseTowerScript currentGridTowerObject;

    private void Awake()
    {    
        ShopÝtemData.Event_UpdateCurrentBuyedTowerID += TowerFactory_UpdateCurrentBuyedTower;

        GridObject.Event_UpdateCurrentGridobject += TowerFactory_UpdateCurrentGridTower;

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
    private void TowerFactory_UpdateCurrentBuyedTower(TowerDataSO currentBuyedTowerSO)
    {
        BuyedTowerID = currentBuyedTowerSO.towerID;
        Event_UpdateCurrentBuyedTower?.Invoke(simpleTowers[BuyedTowerID]); 
    }
    public void TowerFactory_UpdateCurrentGridTower(GridObject gridObject)
    {
        if (!gridObject)
            return;

        currentGridTowerObject = gridObject.GetTowerObjectOnGrid();
        CheckGridTowerAndBuyedTowerCombinable();
    }
    private void CheckGridTowerAndBuyedTowerCombinable()
    {
        Event_CurrentGridTowerUpgradeVersion?.Invoke(null);
        ChangeCursorManager.Instance.ChangeCursorTexture(ChangeCursorManager.CursorType.normal);

        if (currentGridTowerObject && !string.IsNullOrEmpty(BuyedTowerID))
        {
            var TowerData = currentGridTowerObject.GetTowerData();
            var UpgradedTowerData = TowerData.GetTowerUpgradeVersion(simpleTowers[BuyedTowerID].GetTowerData());

            if (!UpgradedTowerData)
                return;

            ChangeCursorManager.Instance.ChangeCursorTexture(ChangeCursorManager.CursorType.upgradeAvailable);
            Event_CurrentGridTowerUpgradeVersion?.Invoke(AdvanceTowers[UpgradedTowerData.towerID]);
        }
    }
    public BaseTowerScript SetRequestTower(string towerID) => AdvanceTowers[towerID];


}
