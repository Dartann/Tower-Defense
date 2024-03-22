using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public delegate BaseTowerScript RequestTowerUpgrade(string TowerID);
    public static RequestTowerUpgrade Event_RequestTowerUpgrade;

    GridObject currentGridTileObject;
    BaseTowerScript currentBuyedTower;
    BaseTowerScript currentBuyedTowerUpgradeVersion;

    bool isInBuildingMode = true;

    private void PlacementManager_UpdateCurrentGridObject(GridObject gridobject)
    {
        //Can be null if outside grid
        currentGridTileObject = gridobject;
        ChangeCursorManager.Instance.ChangeCursorTexture(ChangeCursorManager.CursorType.normal);
        GetTowerCombineVersion();
    }
    private void PlacementManager_UpdateCurrentBuyedTower(BaseTowerScript buyedTower) => currentBuyedTower = buyedTower;
    private void Awake()
    {
        GridObject.Event_UpdateCurrentGridobject += PlacementManager_UpdateCurrentGridObject;

        TowerFactory.Event_UpdateCurrentBuyedTower += PlacementManager_UpdateCurrentBuyedTower;
    }

    private void Update()
    {
        Testputmethod();
    }
    private void Testputmethod()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanBuild())
            BuildOnCurrentGridTile();
        
    }
    private void BuildOnCurrentGridTile(){

        if (!currentGridTileObject.IsThereTowerOnGrid)
        {
            var NewTower = Instantiate(currentBuyedTower, currentGridTileObject.transform.position, Quaternion.identity);
            currentGridTileObject.SetTowerObjectOnGrid(NewTower);
            GetTowerCombineVersion();
            return;
        }     
        if (currentBuyedTowerUpgradeVersion)
        {
            var NewTower = Instantiate(currentBuyedTowerUpgradeVersion, currentGridTileObject.transform.position, Quaternion.identity);
            currentGridTileObject.SetTowerObjectOnGrid(NewTower);
            return;
        }

    }
    private void GetTowerCombineVersion()
    {

        currentBuyedTowerUpgradeVersion = null;

        if (!currentGridTileObject || !currentGridTileObject.IsThereTowerOnGrid || !currentBuyedTower)    
            return;

        var TowerData = currentGridTileObject.GetTowerObjectOnGrid().GetTowerData();
        var UpgradedTowerData = TowerData.GetTowerUpgradeVersion(currentBuyedTower.GetTowerData());

        if (!UpgradedTowerData)
            return;

        currentBuyedTowerUpgradeVersion = Event_RequestTowerUpgrade.Invoke(UpgradedTowerData.towerID);

        ChangeCursorManager.Instance.ChangeCursorTexture(ChangeCursorManager.CursorType.avaible);
    }
    private bool CanBuild() => isInBuildingMode && currentGridTileObject && currentBuyedTower;

}
