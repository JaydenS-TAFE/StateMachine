using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _chaseDistance;

    [SerializeField] private Vector2[] _waypoints;
    private int _currentWaypointGoalIndex;

    private Vector2 _roamPosition;
    private float _roamPositionChangeTimer;

    private void Awake()
    {
        if (_waypoints.Length == 0)
        {
            _waypoints = new Vector2[Random.Range(3, 11)];
            for (int i = 0; i < _waypoints.Length; i++)
                _waypoints[i] = transform.position + (Vector3)(Random.insideUnitCircle * Random.Range(2f, 12f));
        }
        
    }

    private void Start()
    {
        _currentWaypointGoalIndex = FindClosestWaypoint();
    }

    public void Partrol()
    {
        MoveTowardsPosition(_waypoints[_currentWaypointGoalIndex]);
        if (((Vector3)_waypoints[_currentWaypointGoalIndex] - transform.position).magnitude <= _stopDistance)
        {
            _currentWaypointGoalIndex++;
            if (_currentWaypointGoalIndex >= _waypoints.Length)
                _currentWaypointGoalIndex = 0;
        }
    }
    public void ChaseTarget()
    {
        MoveTowardsPosition(_target.position);
    }
    public void Roam(float repositionTimeRangeMin, float repositionTimeRangeMax, float speedScale)
    {
        _roamPositionChangeTimer -= Time.deltaTime;
        if (_roamPositionChangeTimer <= 0f)
        {
            _roamPosition = transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            _roamPositionChangeTimer += Random.Range(repositionTimeRangeMin, repositionTimeRangeMax);
        }
        MoveTowardsPosition(_roamPosition, speedScale);
    }
    public void Roam(float repositionTimeRangeMin, float repositionTimeRangeMax)
    {
        Roam(repositionTimeRangeMin, repositionTimeRangeMax, 1f);
    }
    public void StopRoaming()
    {
        _currentWaypointGoalIndex = FindClosestWaypoint();
    }

    private void MoveTowardsPosition(Vector3 goal, float speedScale)
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, _baseSpeed * speedScale * Time.deltaTime);
    }
    private void MoveTowardsPosition(Vector3 goal)
    {
        MoveTowardsPosition(goal, 1f);
    }

    public bool IsTargetInRange()
    {
        if (_target == null)
            return false;
        return Vector2.Distance(transform.position, _target.position) <= _chaseDistance;
    }

    public int FindClosestWaypoint()
    {
        return FindClosestWaypoint(transform.position);
    }
    public int FindClosestWaypoint(Vector2 position)
    {
        if (_waypoints.Length == 0)
        {
            Debug.LogError("There are no waypoints to find the closest to");
            return -1;
        }

        float smallestDistance = float.MaxValue;
        int smallestDistanceWaypoint = -1;
        for (int i = 0; i < _waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, _waypoints[i]);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                smallestDistanceWaypoint = i;
            }
        }
        return smallestDistanceWaypoint;
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
