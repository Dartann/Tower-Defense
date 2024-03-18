using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    //Used in TowerFactory script
    public delegate BaseTowerScript CreateTower(string selectedTowerID, GridObject currentSelectedGridObject);
    public static CreateTower Event_requestNewTower;

    public delegate bool CheckTowerCanUpgraded(string selectedTowerID, GridObject currentSelectedGridObject);
    public static CheckTowerCanUpgraded Event_isTowerUpgradeExist;

    GameObject currentGridTileObject;
    string currentSelectedTowerID;

    bool isInBuildingMode = true;
    private void PlacementManager_UpdateCurrentGridObject(GameObject gridobject) { currentGridTileObject = gridobject; }

    private void Awake()
    {
        Shop.Event_UpdateCurrentTowerObjectID += PlacementManager_UpdateCurrentSelectedTowerID;

        GridObject.Event_UpdateCurrentGridobject += PlacementManager_UpdateCurrentGridObject;
        GridObject.Event_CheckTowerHasUpgrade += PlacementManager_CanUpgradeTower;
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
    private void PlacementManager_CanUpgradeTower()
    {
        if (CanBuild())
        {
            var gridData = currentGridTileObject.GetComponent<GridObject>();
            Event_isTowerUpgradeExist?.Invoke(currentSelectedTowerID, gridData);
        }
    }

    private void BuildOnCurrentGridTile(){

        var gridData = currentGridTileObject.GetComponent<GridObject>();

        var createdTower = Event_requestNewTower?.Invoke(currentSelectedTowerID, gridData);

        if (!createdTower)
            return;

        gridData.SetTowerObjectOnGrid(createdTower);

    }
    private void PlacementManager_UpdateCurrentSelectedTowerID(BaseTowerScript currentSelectedTowerScript) => 
        currentSelectedTowerID = currentSelectedTowerScript.GetTowerData().towerID;

    private bool CanBuild() => isInBuildingMode && currentGridTileObject && !string.IsNullOrEmpty(currentSelectedTowerID);

}
