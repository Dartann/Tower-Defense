using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelTower : BaseTowerScript
{
    protected override void FireBullet()
    {
        for (int i = 0; i < bulletSpawnPosition.Length; i++) { 
            Instantiate(towerBulletPrefab, bulletSpawnPosition[i].position, Quaternion.Euler(bulletSpawnPosition[i].transform.rotation.eulerAngles));
        }
    }
}
