using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursorManager : MonoBehaviour
{
    public static ChangeCursorManager Instance { get; private set; }
    public enum CursorType
    {
        normal,
        upgradeAvailable,
    }

    [Serializable]
    public struct CursorData
    {
        public Texture2D cursorTexture;
        public CursorType cursorType;
    }

    public List<CursorData> cursorData = new();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
          
    }
    public void ChangeCursorTexture(CursorType cursorType)
    {   
        if (cursorType == CursorType.normal){
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        foreach (CursorData cursorTexture in cursorData)
        {
            if (cursorTexture.cursorType == cursorType)
            {
                Cursor.SetCursor(cursorTexture.cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
            
        }     
    }

}
