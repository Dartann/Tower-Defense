using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopİtemData : MonoBehaviour
{
    public static Action<TowerDataSO> Event_UpdateCurrentBuyedTowerID;

    [SerializeField]
    List<TowerDataSO> unlockedTowers = new();

    public void AddUnlockedTower(TowerDataSO unlockedTower) => unlockedTowers.Add(unlockedTower);
    public bool İsTowerUnlocked(string towerID)
    {
        var tower = unlockedTowers.Find(unlockedTower => unlockedTower.towerID == towerID);
        if(tower == null)
            return false;

        return true;
    }

    // used on Canvas/buttonLayout buttons
    public void GetTower(TowerDataSO towerData)
    {
        if (İsTowerUnlocked(towerData.towerID))
            Event_UpdateCurrentBuyedTowerID.Invoke(towerData);
        else
            Debug.Log("Tower not unlocked yet");      
    }

}
