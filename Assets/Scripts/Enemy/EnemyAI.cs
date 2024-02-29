using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    List<Transform> _pathTargets;
    Vector2 _currentTargetPosition;

    int pathIndex = 0;

    int speed;
    private void Start() => speed = GetComponent<EnemyData>().GetSpeed();
    private void FixedUpdate(){
        transform.position = Vector2.MoveTowards(transform.position, _currentTargetPosition, speed * Time.deltaTime);
        UpdateTarget();
    }
    
    public void SetTargets(List<Transform> targets) {
        _pathTargets = targets;
        SetFirstTarget();
    }

    private void SetFirstTarget() => _currentTargetPosition = _pathTargets[0].position;
    private void UpdateTarget(){

        if (Vector2.Distance(gameObject.transform.position, _currentTargetPosition) <= 0.01f)
        {
            pathIndex++;

            if (pathIndex > _pathTargets.Count - 1)
            {
                Destroy(gameObject);
                return;
            }

            _currentTargetPosition = _pathTargets[pathIndex].position;
        }
    }
}
