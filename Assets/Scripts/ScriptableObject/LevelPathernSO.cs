using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "Level/LevelHolder")]
[Serializable]
public class LevelPathernSO : ScriptableObject
{
    public LevelData levels = new LevelData();

    [Serializable]
    public class LevelData
    {
        public int level;
        public int levelHeight;
        public int levelWidth;
        public List<LevelPathInformation> gridPath;
    }

    [Serializable]
    public struct LevelPathInformation
    {
        public Vector2Int pathStartPosition;
        public Vector2Int pathEndPosition;
        public bool isMovementOnX;
    }

    public Vector2 GetLevelStartPosition(int level)
    {         
        if(level == levels.level)
            return levels.gridPath[0].pathStartPosition;       
        
        return Vector2.zero;
    }

}
