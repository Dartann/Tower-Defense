using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject enemyPrefab;

    List<Transform> _currentLevelPath;
    Vector2 _enemySpawnPosition;

    float _currentWave = 0.5f;
    float _enemySpawnSpeed = 0.5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) 
            StartCoroutine(SpawnEnemy());
    }

    public void GetCurrentLevel›nformation(Vector2 enemySpawnPosition, List<Transform> currentLevelAIPath)
    {
        _enemySpawnPosition = enemySpawnPosition;
        _currentLevelPath = currentLevelAIPath;
    }

    IEnumerator SpawnEnemy()
    {
        float enemyCount = Mathf.Round(2 * _currentWave);

        for(int count = 1; count <= enemyCount; count++)
        {
            yield return new WaitForSeconds(_enemySpawnSpeed);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, _enemySpawnPosition, Quaternion.identity);
            spawnedEnemy.GetComponent<EnemyAI>().SetTargets(_currentLevelPath);
        }

        _currentWave += 0.75f;
        _enemySpawnSpeed -= 0.05f;

    }
}
