using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{  
    public static Action<BaseTowerScript> Event_UpdateCurrentTowerObjectID;

    public void Buy(BaseTowerScript buyedObjectScript)
    {
        Event_UpdateCurrentTowerObjectID.Invoke(buyedObjectScript);
    }
}
