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
    [SerializeField] Camera cam;

    [Header("PlacementCheckLayers")]
    [SerializeField] LayerMask gridLayer;
    [SerializeField] LayerMask towerLayer;

    [Header("Prefab")]
    [SerializeField] GameObject towerPrefab;

    [Header("Placement Cursor")]
    [SerializeField] GameObject cursorObject;

    GameObject currentGridTileObject;

    public bool isInBuildingMode = true;
    public bool canBuildOnTile = false;



    private void Update()
    {
        GetcurrentGridTile();
        Testputmethod();
    }
    private void GetcurrentGridTile()
    {
        RaycastHit2D currentGridTile = Physics2D.Raycast(GetMousePosition(), Vector2.zero, 0.1f, gridLayer , -100 , 100);

        if (currentGridTile.collider == null)
            return;

        if (CanBuildOnPosition())
        {
            cursorObject.transform.position = currentGridTile.collider.transform.position;
            currentGridTileObject = currentGridTile.collider.gameObject;
        }
    }
    private bool CanBuildOnPosition()
    {
        RaycastHit2D currentGridTile = Physics2D.Raycast(GetMousePosition(), Vector2.zero, 0.1f, towerLayer, -100, 100);
        
        if(currentGridTile.collider == null)
            return canBuildOnTile = true;

        return canBuildOnTile = false;
    }
    private Vector2 GetMousePosition()
    {
       Vector2 position = cam.ScreenToWorldPoint(Input.mousePosition);
       return new(MathF.Round(position.x), Mathf.Round(position.y));
    }
    private void Testputmethod()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isInBuildingMode && canBuildOnTile)
        {
           GameObject createdTower = Instantiate(towerPrefab, currentGridTileObject.transform.position, Quaternion.identity);
           createdTower.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
