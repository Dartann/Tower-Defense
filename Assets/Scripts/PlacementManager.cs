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
    [SerializeField] TowerFactory towerFactory;
   
    GameObject currentGridTileObject;

    bool isInBuildingMode = true;

    private void PlacementManager_UpdateCurrentGridObject(GameObject gridobject) { currentGridTileObject = gridobject; }
    private void Awake()
    {
        GridObject.Event_UpdateCurrentGridobject += PlacementManager_UpdateCurrentGridObject;
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

        var gridData = currentGridTileObject.GetComponent<GridObject>();

        var createdTower = towerFactory.InstantiateTower(gridData);

        if (createdTower == null)
            return;

        var NewTower = Instantiate(createdTower, gridData.transform.position, Quaternion.identity);
        gridData.SetTowerObjectOnGrid(NewTower);

    }
    private bool CanBuild() => isInBuildingMode && currentGridTileObject && towerFactory.CurrentSelectedTower;

}
