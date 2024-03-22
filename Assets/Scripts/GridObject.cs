using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public static Action<GridObject> Event_UpdateCurrentGridobject;

    BaseTowerScript towerObjectOnGrid;
    public bool IsThereTowerOnGrid => towerObjectOnGrid;
    public void SetTowerObjectOnGrid(BaseTowerScript transform){
        DestroyOldTowerObject();
        towerObjectOnGrid = transform;
    }
    public BaseTowerScript GetTowerObjectOnGrid(){
        if(IsThereTowerOnGrid)
            return towerObjectOnGrid;

        return null;
    }
    void DestroyOldTowerObject(){
        if (!IsThereTowerOnGrid)
            return;

        Destroy(towerObjectOnGrid.gameObject);
    }

    private void OnMouseEnter() => Event_UpdateCurrentGridobject?.Invoke(this);
    private void OnMouseExit() => Event_UpdateCurrentGridobject.Invoke(null);  
    private void OnMouseOver()
    {
      
        //Add hightlight when on grid
    }

}
