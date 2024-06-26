using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelPathernSO))]
public class LevelPathernSOEditor : Editor
{
    SerializedProperty _levels;

    SerializedProperty _level;
    SerializedProperty _levelHeight;
    SerializedProperty _levelWidth;

    SerializedProperty _gridPath;
    SerializedProperty _arraySize;

    SerializedProperty currentİndex_StartPosition;
    SerializedProperty currentİndex_EndPosition;
    SerializedProperty currentİndex_isMovementOnX;

    SerializedProperty firstİsMovementBool;

    bool show;
    bool[] arrayShow;

    private void OnEnable()
    {
        _levels = serializedObject.FindProperty("levels");

        _level = _levels.FindPropertyRelative("level");
        _levelHeight = _levels.FindPropertyRelative("levelHeight");
        _levelWidth = _levels.FindPropertyRelative("levelWidth");

        _gridPath = _levels.FindPropertyRelative("gridPath");
        _arraySize = _gridPath.FindPropertyRelative("Array.size");

        arrayShow = new bool[_arraySize.intValue];
        for (int i = 0; i < arrayShow.Length; i++)
            arrayShow[i] = true;
      
        Debug.Log(arrayShow[0]);

        firstİsMovementBool = _gridPath.GetArrayElementAtIndex(0).FindPropertyRelative("isMovementOnX");
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        BuildInspector();    
        DrawCurrentMap();

        if (EditorGUI.EndChangeCheck()){
            CheckİnspectorStatsCorrect();
            serializedObject.ApplyModifiedProperties();
        }
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
        EditorGUILayout.BeginHorizontal();
        show = EditorGUILayout.Foldout(show, "Grid Paths");

        EditorGUIUtility.labelWidth = 30;
        EditorGUILayout.PropertyField(_arraySize);

        EditorGUIUtility.labelWidth = 0;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (show){
            UpdateArrayShowList(_arraySize.intValue);

            for (int i = 0; i < _gridPath.arraySize; i++)
            {
                EditorGUI.indentLevel += 1;

                arrayShow[i] = EditorGUILayout.Foldout(arrayShow[i], $"Path {i + 1}");
                
                if (arrayShow[i])
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUI.BeginDisabledGroup(i > 0);
                    EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathStartPosition"));
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("pathEndPosition"));

                    EditorGUI.BeginDisabledGroup(i > 0);
                    EditorGUILayout.PropertyField(_gridPath.GetArrayElementAtIndex(i).FindPropertyRelative("isMovementOnX"));
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel -= 1;
                }
                EditorGUI.indentLevel -= 1;
            }

        }      
    }
    private void UpdateArrayShowList(int arraysize)
    {
        if (Equals(arrayShow.Length, arraysize))
            return;

        arrayShow = new bool[arraysize];

        for (int i = 0; i < arrayShow.Length; i++)
            arrayShow[i] = true;
    }
    // çalışmıyor ya da çok hızlı çalıştığı için gözükmüyor
    private bool CheckCoordinatesİsCorrect(int currentİndex)
    {
        if (currentİndex_StartPosition.vector2IntValue.x < 0 || currentİndex_StartPosition.vector2IntValue.x > _levelWidth.intValue || currentİndex_StartPosition.vector2IntValue.y < 0 || currentİndex_StartPosition.vector2IntValue.y > _levelHeight.intValue ||
           currentİndex_EndPosition.vector2IntValue.x < 0 || currentİndex_EndPosition.vector2IntValue.x > _levelWidth.intValue || currentİndex_EndPosition.vector2IntValue.y < 0 || currentİndex_EndPosition.vector2IntValue.y > _levelHeight.intValue)
        {
            EditorGUILayout.HelpBox($" Path {currentİndex + 1} koordinatında hata mevcut." +
                $" \n\n Kordinatın değeri 0'dan küçük ya da Grid alanından büyük olamaz!", MessageType.Error);

            return false;
        }
        return true;
    }
    private void CheckArraySizeİsCorrect()
    {
        if (_arraySize.intValue < 2)
            _arraySize.intValue = 2;
    }
    private void CorrectMovementBool()
    {
        int value = firstİsMovementBool.boolValue ? 0 : 1;

        for (int gridpath_index = 0; gridpath_index < _gridPath.arraySize; gridpath_index++)
        {
            var currentPathIndex = _gridPath.GetArrayElementAtIndex(gridpath_index);
            var isMovementOnXBool = currentPathIndex.FindPropertyRelative("isMovementOnX");

            if(gridpath_index % 2 == value)
                isMovementOnXBool.boolValue = true;
            else
                isMovementOnXBool.boolValue = false;

        }        
    }
    private void CheckİnspectorStatsCorrect()
    {
        CheckArraySizeİsCorrect();
        CorrectMovementBool();
    
        for (int gridpath_index = 0; gridpath_index < _gridPath.arraySize; gridpath_index++)
        {
            var currentPathIndex = _gridPath.GetArrayElementAtIndex(gridpath_index);

            currentİndex_StartPosition = currentPathIndex.FindPropertyRelative("pathStartPosition");
            currentİndex_EndPosition = currentPathIndex.FindPropertyRelative("pathEndPosition");
            currentİndex_isMovementOnX = currentPathIndex.FindPropertyRelative("isMovementOnX");

            if(!CheckCoordinatesİsCorrect(gridpath_index))
                return;
              
            UpdateStatsBetweenPaths();

            if (gridpath_index + 1 == _gridPath.arraySize)
                continue;

            var next_PathStartPosition = _gridPath.GetArrayElementAtIndex(gridpath_index + 1).FindPropertyRelative("pathStartPosition");

            if (currentİndex_EndPosition.vector2IntValue != next_PathStartPosition.vector2IntValue)
                next_PathStartPosition.vector2IntValue = currentİndex_EndPosition.vector2IntValue;
            
        }
       
    }

    private void UpdateStatsBetweenPaths()
    {       
        currentİndex_EndPosition.vector2IntValue = currentİndex_isMovementOnX.boolValue ? 
            new Vector2Int(currentİndex_EndPosition.vector2IntValue.x, currentİndex_StartPosition.vector2IntValue.y):
            new Vector2Int(currentİndex_StartPosition.vector2IntValue.x, currentİndex_EndPosition.vector2IntValue.y);
    }
}
    