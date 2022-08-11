using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AIAgent))]
public class StateMachine : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Search,
        Panic,
    }

    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private State _state;

    private AIAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<AIAgent>();
    }

    private void Start()
    {
        NextState();
    }

    private void NextState()
    {
        _stateText.text = $"{_state}";
        switch (_state)
        {
            default: Debug.LogWarning("State does not exist in state machine, stopping state machine"); break;
            case State.Patrol:  StartCoroutine(PatrolState()); _stateText.text = "..."; break;
            case State.Chase:   StartCoroutine(ChaseState());  _stateText.text = "!";   break;
            case State.Search:  StartCoroutine(SearchState()); _stateText.text = "?";   break;
            case State.Panic:  StartCoroutine(PanicState());  _stateText.text = "?!";   break;
        }

    }

    #region States
    private IEnumerator PatrolState()
    {
        Debug.Log("Patroling");
        while (_state == State.Patrol)
        {
            _agent.Partrol();
            if (_agent.IsTargetInRange())
                _state = State.Chase;
            yield return null;
        }
        Debug.Log("No longer patroling");
        NextState();
    }
    private IEnumerator ChaseState()
    {
        Debug.Log("Chasing");
        while (_state == State.Chase)
        {
            _agent.ChaseTarget();
            if (!_agent.IsTargetInRange())
                _state = State.Search;
            yield return null;
        }
        Debug.Log("No longer Chasing");
        NextState();
    }
    private IEnumerator SearchState()
    {
        Debug.Log("Searching");
        float searchTime = 5f;
        while (_state == State.Search)
        {
            _agent.Roam(0.5f, 3f);
            searchTime -= Time.deltaTime;
            if (searchTime <= 0f)
            {
                _agent.StopRoaming();
                _state = State.Patrol;
            }
            if (_agent.IsTargetInRange())
                _state = State.Chase;
            yield return null;
        }
        Debug.Log("No longer Searching");
        NextState();
    }
    private IEnumerator PanicState()
    {
        Debug.Log("Panicing");
        float panicTime = 5f;
        while (_state == State.Panic)
        {
            _agent.Roam(0.1f, 0.25f, 2f);
            panicTime -= Time.deltaTime;
            if (panicTime <= 0f)
            {
                _agent.StopRoaming();
                _state = State.Patrol;
            }
            yield return null;
        }
        Debug.Log("No longer panicing");
        NextState();
    }
    #endregion
}
