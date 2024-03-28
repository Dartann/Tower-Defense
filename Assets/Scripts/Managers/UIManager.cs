using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Transform currentCatagory;
    public void SetActiveSingleUI(Transform UIobject)
    {
        if (UIobject == null)
        {
            Debug.Log("Object Not exist");
            return;
        }

        if (UIobject.gameObject.activeSelf)
            UIobject.gameObject.SetActive(false);
        else
            UIobject.gameObject.SetActive(true);

    }
    public void SetActiveShopCategorys(Transform UIobject)
    {
        if (!currentCatagory)
        {
            currentCatagory = UIobject;
            UIobject.gameObject.SetActive(true);
            return;
        }
        currentCatagory.gameObject.SetActive(false);
        currentCatagory = UIobject;
        currentCatagory.gameObject.SetActive(true);



    }

}
