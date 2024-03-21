using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static ChangeCursorManager;

public class ChangeCursorManager : MonoBehaviour
{
    public enum CursorType
    {
        normal,
        avaible,
        notAvaible,  
    }

    [Serializable]
    public struct CursorData
    {
        public Texture2D cursorTexture;
        public CursorType cursorType;
    }

    public List<CursorData> cursorData = new();

    private void OnEnable() => GridObject.Event_ChangeCursor += ChangeCursorTexture;
    private void OnDisable() => GridObject.Event_ChangeCursor -= ChangeCursorTexture;

    private void ChangeCursorTexture(CursorType cursorType, Vector2 objectPosition)
    {
        if(cursorType == CursorType.normal){
            Cursor.SetCursor(null, objectPosition, CursorMode.Auto);
            return;
        }

        foreach (CursorData texture in cursorData)
        {
            if (texture.cursorType == cursorType)
                Cursor.SetCursor(texture.cursorTexture, objectPosition,CursorMode.Auto);
            
        }     
    }

}
