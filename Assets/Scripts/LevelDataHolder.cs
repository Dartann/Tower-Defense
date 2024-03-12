using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private LevelPathernSO[] SO_Level_Patherns;

    private Dictionary<int, LevelPathernSO> levelPatherns = new();
    private void Awake()
    {
        foreach (LevelPathernSO levelData in SO_Level_Patherns) 
            levelPatherns.Add(levelData.levels.level, levelData);

        DontDestroyOnLoad(this);
    }

    public LevelPathernSO GetCurrentLevelData(int Level)
    {
        return levelPatherns[Level];
    }
}
