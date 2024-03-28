using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(LevelPathernSO))]
public class LevelPathernSOEditor : Editor
{
    SerializedProperty _levels;

    SerializedProperty _level;
    SerializedProperty _levelHeight;
    SerializedProperty _levelWidth;

    SerializedProperty _gridPath;
    
    SerializedProperty _pathStartPosition;
    SerializedProperty _pathEndPosition;
    SerializedProperty _isMovementOnX;

    SerializedProperty _nextPathStartPosition;

    bool show;
    private void OnEnable()
    {
        _levels = serializedObject.FindProperty("levels");

        _level = _levels.FindPropertyRelative("level");
        _levelHeight = _levels.FindPropertyRelative("levelHeight");
        _levelWidth = _levels.FindPropertyRelative("levelWidth");

        _gridPath = _levels.FindPropertyRelative("gridPath");
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();

        //base.OnInspectorGUI();
        BuildInspector();

        CheckStartAndEndPositionBetweenPaths();
        DrawCurrentMap();

        if(EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }   
    private void DrawCurrentMap()
    {
        EditorGUILayout.Space(25);

        if (GUILayout.Button("Preview Current Map"))
        {
            for (int i = 0; i < _gridPath.arraySize; i++)
            {
                //grid Path
                var startPosition = _gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathStartPosition");
                var endPosition = _gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathEndPosition");

                Debug.DrawLine((Vector3Int)startPosition.vector2IntValue, (Vector3Int)endPosition.vector2IntValue, color: Color.green, 0.1f);

            }
            // TODO: currently grid area is bit offsize, need fix later
            // Grid Area
            Debug.DrawLine(Vector3.zero, new Vector3(_levelWidth.intValue, 0, 0), color: Color.green, 0.1f);
            Debug.DrawLine(Vector3.zero, new Vector3(0, _levelHeight.intValue, 0), color: Color.green, 0.1f);
            Debug.DrawLine(new Vector3(0, _levelHeight.intValue, 0), new Vector3(_levelWidth.intValue, _levelHeight.intValue, 0), color: Color.green, 0.1f);
            Debug.DrawLine(new Vector3(_levelWidth.intValue, 0, 0), new Vector3(_levelWidth.intValue, _levelHeight.intValue, 0), color: Color.green, 0.1f);
            SceneView.RepaintAll();
        }
    }
    private void BuildInspector()
    {
        EditorGUILayout.LabelField("Level Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_level);
        EditorGUILayout.PropertyField(_levelHeight);
        EditorGUILayout.PropertyField(_levelWidth);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Grid Path Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel += 1;

        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 50;
        show = EditorGUILayout.Foldout(show, "Grid Paths");
        EditorGUILayout.PropertyField(_gridPath.FindPropertyRelative("Array.size"));
        EditorGUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;

        EditorGUILayout.Space();
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
            EditorGUILayout.HelpBox("Girilen kordinat 0'dan küçük ya da Grid alanýndan büyük olamaz!", MessageType.Error);
        }

        _pathEndPosition.vector2IntValue = _isMovementOnX.boolValue ? new Vector2Int(_pathEndPosition.vector2IntValue.x, _pathStartPosition.vector2IntValue.y) : new Vector2Int(_pathStartPosition.vector2IntValue.x, _pathEndPosition.vector2IntValue.y);


    }
    private void CheckStartAndEndPositionBetweenPaths()
    {
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
    