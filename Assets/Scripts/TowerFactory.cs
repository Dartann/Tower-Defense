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
    public static Action<BaseTowerScript> Event_UpdateCurrentBuyedTower;

    [SerializeField] BaseTowerScript[] basePrefabs;
    [SerializeField] BaseTowerScript[] upgradePrefabs;

    private Dictionary<string, BaseTowerScript> simpleTowers = new();
    private Dictionary<string, BaseTowerScript> AdvanceTowers = new();

    string BuyedTowerID;

    private void Awake()
    {    
        Shop.Event_UpdateCurrentBuyedTowerID += TowerFactory_UpdateCurrentSelectedTowerID;

        PlacementManager.Event_RequestTowerUpgrade += SetRequestTower;

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
        BuyedTowerID = currentSelectedTowerScript.GetTowerData().towerID;
        Event_UpdateCurrentBuyedTower?.Invoke(simpleTowers[BuyedTowerID]);
    }

    public BaseTowerScript SetRequestTower(string towerID) => AdvanceTowers[towerID];

}
