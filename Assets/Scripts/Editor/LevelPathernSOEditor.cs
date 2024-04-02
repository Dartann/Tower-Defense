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

    SerializedProperty currentÝndex_StartPosition;
    SerializedProperty currentÝndex_EndPosition;
    SerializedProperty currentÝndex_isMovementOnX;

    SerializedProperty firstÝsMovementBool;

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

        firstÝsMovementBool = _gridPath.GetArrayElementAtIndex(0).FindPropertyRelative("isMovementOnX");
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        serializedObject.Update();

        BuildInspector();    
        DrawCurrentMap();

        if (EditorGUI.EndChangeCheck()){
            CheckÝnspectorStatsCorrect();
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
    // çalýþmýyor ya da çok hýzlý çalýþtýðý için gözükmüyor
    private bool CheckCoordinatesÝsCorrect(int currentÝndex)
    {
        if (currentÝndex_StartPosition.vector2IntValue.x < 0 || currentÝndex_StartPosition.vector2IntValue.x > _levelWidth.intValue || currentÝndex_StartPosition.vector2IntValue.y < 0 || currentÝndex_StartPosition.vector2IntValue.y > _levelHeight.intValue ||
           currentÝndex_EndPosition.vector2IntValue.x < 0 || currentÝndex_EndPosition.vector2IntValue.x > _levelWidth.intValue || currentÝndex_EndPosition.vector2IntValue.y < 0 || currentÝndex_EndPosition.vector2IntValue.y > _levelHeight.intValue)
        {
            EditorGUILayout.HelpBox($" Path {currentÝndex + 1} koordinatýnda hata mevcut." +
                $" \n\n Kordinatýn deðeri 0'dan küçük ya da Grid alanýndan büyük olamaz!", MessageType.Error);

            return false;
        }
        return true;
    }
    private void CheckArraySizeÝsCorrect()
    {
        if (_arraySize.intValue < 2)
            _arraySize.intValue = 2;
    }
    private void CorrectMovementBool()
    {
        int value = firstÝsMovementBool.boolValue ? 0 : 1;

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
    private void CheckÝnspectorStatsCorrect()
    {
        CheckArraySizeÝsCorrect();
        CorrectMovementBool();
    
        for (int gridpath_index = 0; gridpath_index < _gridPath.arraySize; gridpath_index++)
        {
            var currentPathIndex = _gridPath.GetArrayElementAtIndex(gridpath_index);

            currentÝndex_StartPosition = currentPathIndex.FindPropertyRelative("pathStartPosition");
            currentÝndex_EndPosition = currentPathIndex.FindPropertyRelative("pathEndPosition");
            currentÝndex_isMovementOnX = currentPathIndex.FindPropertyRelative("isMovementOnX");

            if(!CheckCoordinatesÝsCorrect(gridpath_index))
                return;
              
            UpdateStatsBetweenPaths();

            if (gridpath_index + 1 == _gridPath.arraySize)
                continue;

            var next_PathStartPosition = _gridPath.GetArrayElementAtIndex(gridpath_index + 1).FindPropertyRelative("pathStartPosition");

            if (currentÝndex_EndPosition.vector2IntValue != next_PathStartPosition.vector2IntValue)
                next_PathStartPosition.vector2IntValue = currentÝndex_EndPosition.vector2IntValue;
            
        }
       
    }

    private void UpdateStatsBetweenPaths()
    {       
        currentÝndex_EndPosition.vector2IntValue = currentÝndex_isMovementOnX.boolValue ? 
            new Vector2Int(currentÝndex_EndPosition.vector2IntValue.x, currentÝndex_StartPosition.vector2IntValue.y):
            new Vector2Int(currentÝndex_StartPosition.vector2IntValue.x, currentÝndex_EndPosition.vector2IntValue.y);
    }
}
    