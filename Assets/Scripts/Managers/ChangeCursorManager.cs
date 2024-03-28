using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursorManager : MonoBehaviour
{
    public static ChangeCursorManager Instance { get; private set; }

    [SerializeField] Camera _camera;
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
        if(cursorType == CursorType.normal){
            Cursor.SetCursor(null, _camera.ScreenToWorldPoint(Input.mousePosition), CursorMode.Auto);
            return;
        }

        foreach (CursorData texture in cursorData)
        {
            if (texture.cursorType == cursorType)
                Cursor.SetCursor(texture.cursorTexture, _camera.ScreenToWorldPoint(Input.mousePosition), CursorMode.Auto);
            
        }     
    }

}
