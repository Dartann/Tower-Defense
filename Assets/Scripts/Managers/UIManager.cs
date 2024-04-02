using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action<bool> Event_ÝnBuildMode;

    [SerializeField] Transform BuildLayOut;

    Transform currentShopCatagory;

    public void SetActiveUI(Transform UIobject)
    {
        if (UIobject == null)
        {
            Debug.Log("Object Not exist");
            return;
        }

        if (IsTransformActive(UIobject))
            UIobject.gameObject.SetActive(false);
        else
            UIobject.gameObject.SetActive(true);

    }
    public void SetActiveShopCategorys(Transform UIobject)
    {
        if (!currentShopCatagory)
        {
            currentShopCatagory = UIobject;
            UIobject.gameObject.SetActive(true);
            return;
        }
        currentShopCatagory.gameObject.SetActive(false);
        currentShopCatagory = UIobject;
        currentShopCatagory.gameObject.SetActive(true);

    }
    public bool IsTransformActive(Transform layout) => layout.gameObject.activeSelf;
    public void DisableBuildMode() => Event_ÝnBuildMode?.Invoke(BuildLayOut.gameObject.activeSelf);

}
