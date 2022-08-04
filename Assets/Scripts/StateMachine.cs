using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Search,
    }

    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private State _state;

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
            case State.Patrol:  StartCoroutine(PatrolState());  break;
            case State.Chase:   StartCoroutine(ChaseState());   break;
            case State.Search:  StartCoroutine(SearchState());  break;
        }

    }

    #region States
    private IEnumerator PatrolState()
    {
        Debug.Log("Patroling");
        while (_state == State.Patrol)
        {
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
            yield return null;
        }
        Debug.Log("No longer Chasing");
        NextState();
    }
    private IEnumerator SearchState()
    {
        Debug.Log("Searching");
        while (_state == State.Search)
        {
            yield return null;
        }
        Debug.Log("No longer Searching");
        NextState();
    }
    #endregion
}
