using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _stopDistance;

    [SerializeField] private Vector2[] _waypoints;
    private int _currentWaypointGoalIndex;

    private void Start()
    {
        if (_waypoints.Length == 0)
        {
            _waypoints = new Vector2[Random.Range(3, 11)];
            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = transform.position + (Vector3)(Random.insideUnitCircle * Random.Range(2f, 12f));
        }
    }

    private void Update()
    {
        MoveTowardsPosition(_waypoints[_currentWaypointGoalIndex]);
        if (((Vector3)_waypoints[_currentWaypointGoalIndex] - transform.position).magnitude <= _stopDistance)
        {
            _currentWaypointGoalIndex++;
            if (_currentWaypointGoalIndex >= _waypoints.Length)
                _currentWaypointGoalIndex = 0;
        }
    }

    private void MoveTowardsPosition(Vector3 goal)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, _baseSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (_waypoints.Length == 0)
            return;

        //Goal Line
        Gizmos.color = new Color(1f, 1f, 1f, 1f -((Mathf.Sin(Time.unscaledTime * 10f) + 1f) * 0.25f));
        Gizmos.DrawLine(transform.position, _waypoints[_currentWaypointGoalIndex]);

        //Lines
        for (int i = 0; i < _waypoints.Length; i++)
        {
            float progress = (float)i / _waypoints.Length;
            Gizmos.color = new Color(progress, 1f - progress, 0f);

            if (i == _waypoints.Length - 1)
                Gizmos.DrawLine(_waypoints[i], _waypoints[0]);
            else
                Gizmos.DrawLine(_waypoints[i], _waypoints[i + 1]);
        }
        //Dots
        for (int i = 0; i < _waypoints.Length; i++)
        {
            float progress = (float)i / _waypoints.Length;
            Gizmos.color = new Color(progress, 1f - progress, 0f);
            Gizmos.DrawSphere(_waypoints[i], 0.25f);
        }

    }
}
