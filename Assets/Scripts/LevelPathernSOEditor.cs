using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(LevelPathernSO))]
public class LevelPathernSOEditor : Editor
{
    SerializedProperty _levels;

    SerializedProperty _levelHeight;
    SerializedProperty _levelWidth;

    SerializedProperty _gridPath;
    
    SerializedProperty _pathStartPosition;
    SerializedProperty _pathEndPosition;
    SerializedProperty _isMovementOnX;

    SerializedProperty _nextPathStartPosition;

    bool show;
    private void OnEnable() => _levels = serializedObject.FindProperty("levels");
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //base.OnInspectorGUI();
        BuildInspector();

        CheckStartAndEndPositionBetweenPaths();

        serializedObject.ApplyModifiedProperties();
    }
    private void BuildInspector()
    {
        EditorGUILayout.PropertyField(_levels.FindPropertyRelative("level"));
        EditorGUILayout.PropertyField(_levels.FindPropertyRelative("levelHeight"));
        EditorGUILayout.PropertyField(_levels.FindPropertyRelative("levelWidth"));

        _gridPath = _levels.FindPropertyRelative("gridPath");

        EditorGUILayout.BeginHorizontal();
        show = EditorGUILayout.Foldout(show, "Gridpath");
        EditorGUILayout.PropertyField(_gridPath.FindPropertyRelative("Array.size"));
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel += 1;
        if (show){
           for (int i = 0; i < _gridPath.arraySize; i++)
           {
                EditorGUI.BeginDisabledGroup(i > 0);
                EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathStartPosition"));
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathEndPosition"));
                EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("isMovementOnX"));
           }
        }
        EditorGUI.indentLevel -= 1;
    }  
    private void CheckGivenCoordinatesCorrectorNot()
    {     
       
        if(_pathStartPosition.vector2IntValue.x < 0 || _pathStartPosition.vector2IntValue.x > _levelWidth.intValue || _pathStartPosition.vector2IntValue.y < 0 || _pathStartPosition.vector2IntValue.y > _levelHeight.intValue ||
           _pathEndPosition.vector2IntValue.x < 0 || _pathEndPosition.vector2IntValue.x > _levelWidth.intValue || _pathEndPosition.vector2IntValue.y < 0 || _pathEndPosition.vector2IntValue.y > _levelHeight.intValue)
        {
            EditorGUILayout.HelpBox("Girilen " + _gridPath.name + " 0'dan küçük ya da Grid alanýndan büyük olamaz!", MessageType.Error);
        }

        _pathEndPosition.vector2IntValue = _isMovementOnX.boolValue ? new Vector2Int(_pathEndPosition.vector2IntValue.x, _pathStartPosition.vector2IntValue.y) : new Vector2Int(_pathStartPosition.vector2IntValue.x, _pathEndPosition.vector2IntValue.y);


    }
    private void CheckStartAndEndPositionBetweenPaths()
    {
        _levelHeight = _levels.FindPropertyRelative("levelHeight");
        _levelWidth = _levels.FindPropertyRelative("levelWidth");

        _gridPath = _levels.FindPropertyRelative("gridPath");

        for (int gridpath_index = 0; gridpath_index < _gridPath.arraySize; gridpath_index++)
        {
              var currentPathIndex = _gridPath.GetArrayElementAtIndex(gridpath_index);

              _pathStartPosition = currentPathIndex.FindPropertyRelative("pathStartPosition");
              _pathEndPosition = currentPathIndex.FindPropertyRelative("pathEndPosition");
              _isMovementOnX = currentPathIndex.FindPropertyRelative("isMovementOnX");

              CheckGivenCoordinatesCorrectorNot();

              if (gridpath_index + 1 == _gridPath.arraySize)
                  continue;

              _nextPathStartPosition = _gridPath.GetArrayElementAtIndex(gridpath_index + 1).FindPropertyRelative("pathStartPosition");

              if (_pathEndPosition.vector2IntValue != _nextPathStartPosition.vector2IntValue)              
                  _nextPathStartPosition.vector2IntValue = _pathEndPosition.vector2IntValue;               
                
        }
        
    }
}
    