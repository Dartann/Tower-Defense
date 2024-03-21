using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public static Action<GameObject> Event_UpdateCurrentGridobject;
    public static Action<ChangeCursorManager.CursorType, Vector2> Event_ChangeCursor;

    public delegate bool CheckTowerHaveUpgrade(BaseTowerScript tower);
    public static CheckTowerHaveUpgrade Event_CheckTowerHaveUpgrade;


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

    private void OnMouseEnter(){
        Event_UpdateCurrentGridobject?.Invoke(gameObject);

        if (IsThereTowerOnGrid){
           bool isTowerHaveUpgrade = Event_CheckTowerHaveUpgrade.Invoke(towerObjectOnGrid);
           if (isTowerHaveUpgrade)
                Event_ChangeCursor?.Invoke(ChangeCursorManager.CursorType.avaible, towerObjectOnGrid.transform.position);
           else
                Event_ChangeCursor?.Invoke(ChangeCursorManager.CursorType.normal, towerObjectOnGrid.transform.position);
        }
    }
    private void OnMouseExit()
    {
        Event_UpdateCurrentGridobject.Invoke(null);

        if (IsThereTowerOnGrid)
            Event_ChangeCursor?.Invoke(ChangeCursorManager.CursorType.normal, towerObjectOnGrid.transform.position);

    }
       
    private void OnMouseOver()
    {
      
        //Add hightlight when on grid
    }

}
