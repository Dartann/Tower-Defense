using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "Level/LevelHolder")]
public class LevelPathernSO : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();

    [System.Serializable]
    public struct LevelData
    {
        public int level;
        public int levelHeight;
        public int levelWidth;
        public List<LevelPath> gridPath;
    }

    [System.Serializable]
    public struct LevelPath
    {
        public Vector2 roadPosition;
        public bool spawnPathNode;
    } 
}
