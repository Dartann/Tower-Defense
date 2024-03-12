using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{  
    public static Action<BaseTowerScript> updateCurrentBuyedTowerOBject;
    public void Buy(BaseTowerScript tower)
    {
        updateCurrentBuyedTowerOBject.Invoke(tower);
    }
}
