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
    public delegate Transform CreateTower(string towerID, GridObject currentGridTile);
    public static CreateTower createTower;

    [SerializeField] Camera cam;

    [Header("PlacementCheckLayers")]
    [SerializeField] LayerMask gridLayer;

    [Header("Placement Cursor")]
    [SerializeField] GameObject cursorObject;

    GameObject currentGridTileObject;

    bool isInBuildingMode = true;
    string currentSelectedTowerID;
    private void Awake()
    {
        Shop.updateCurrentBuyedTowerOBject += UpdateCurrentSelectedTowerID;
    }

    private void Update()
    {
        GetcurrentGridTile();
        Testputmethod();
    }
    private void GetcurrentGridTile(){
        RaycastHit2D currentGridTile = Physics2D.Raycast(GetMousePositionAsGridPosition(), Vector2.zero, 0.1f, gridLayer , -100 , 100);

        currentGridTileObject = currentGridTile.collider ? currentGridTile.collider.gameObject : null;

        if (!currentGridTileObject)
            return;

        cursorObject.transform.position = currentGridTileObject.transform.position;
    }
    private void BuildOnPosition(){

        if (!currentGridTileObject || string.IsNullOrEmpty(currentSelectedTowerID))
            return;

        var gridData = currentGridTileObject.GetComponent<GridObject>();

        var createdTower = createTower?.Invoke(currentSelectedTowerID, gridData);
        gridData.SetNewTransformObject(createdTower);

    }
    private Vector2 GetMousePositionAsGridPosition(){
       Vector2 position = cam.ScreenToWorldPoint(Input.mousePosition);
       return new(MathF.Round(position.x), Mathf.Round(position.y));
    }
    private void Testputmethod()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isInBuildingMode)
            BuildOnPosition();
        
    }
    private void UpdateCurrentSelectedTowerID(BaseTowerScript data)
    {
        currentSelectedTowerID = data.GetTowerData().towerID;
    }
}
