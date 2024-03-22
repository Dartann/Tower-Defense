using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataSO", menuName = "Tower/TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
    public readonly string towerID = Guid.NewGuid().ToString();

    [Header("Tower Stats")]
    public int damage;
    public int range;
    public float fireRate;
    public int bulletSpeed;

    public List<TowerLinkUps> towerLinks = new();

    [Serializable]
    public class TowerLinkUps
    {
        public TowerDataSO RequiredTower;
        public TowerDataSO UpgradedTower;
    }
    public TowerDataSO GetTowerUpgradeVersion(TowerDataSO BuyedTower){

        var UpgradedTower = towerLinks.Where(LinkData => LinkData.RequiredTower == BuyedTower).FirstOrDefault();

        if (UpgradedTower == null)
            return null;

        return UpgradedTower.UpgradedTower;
    }
}

