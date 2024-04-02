using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class TowerDataSO : ScriptableObject
{
    public readonly string towerID = Guid.NewGuid().ToString();

    [Header("Tower Price")]
    public int RequestedMoney;

    public List<TowerLinkUps> towerLinks = new();

    [Serializable]
    public class TowerLinkUps
    {
        public TowerDataSO RequiredTower;
        public TowerDataSO UpgradedTower;
    }
    public TowerDataSO GetTowerUpgradeVersion(TowerDataSO BuyedTower)
    {

        var linkedTower = towerLinks.Where(LinkData => LinkData.RequiredTower == BuyedTower).FirstOrDefault();

        if (linkedTower == null)
            return null;

        return linkedTower.UpgradedTower;

    }

}

