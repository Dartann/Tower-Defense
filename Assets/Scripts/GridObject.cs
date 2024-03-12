using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    Transform TowerObject;
    public void SetNewTransformObject(Transform transform)
    {
        if (!transform)
            return;

        DestroyOldTowerObject();
        TowerObject = transform;
    }
    public Transform GetOnGridTowerTransform() => TowerObject;
    void DestroyOldTowerObject()
    {
        if (!TowerObject)
            return;

        Destroy(TowerObject.gameObject);
    }


}
