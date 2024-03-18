using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Used in PlacementManager
    public static Action<GameObject> Event_UpdateCurrentGridobject;
    public static Action Event_CheckTowerHasUpgrade;

    BaseTowerScript towerObjectOnGrid;
    public void SetTowerObjectOnGrid(BaseTowerScript transform)
    {
        DestroyOldTowerObject();
        towerObjectOnGrid = transform;
    }
    public BaseTowerScript GetTowerObjectOnGrid()
    {
        if(IsThereTowerOnGrid)
            return towerObjectOnGrid;

        return null;
    }
    public bool IsThereTowerOnGrid => towerObjectOnGrid;
    void DestroyOldTowerObject()
    {
        if (!towerObjectOnGrid)
            return;

        Destroy(towerObjectOnGrid.gameObject);
    }
    private void OnMouseEnter()
    {
        Event_UpdateCurrentGridobject?.Invoke(gameObject);
        Event_CheckTowerHasUpgrade?.Invoke();

    }
    private void OnMouseExit() => Event_UpdateCurrentGridobject.Invoke(null);

    private void OnMouseOver()
    {
        //Add hightlight when on grid
    }

}
