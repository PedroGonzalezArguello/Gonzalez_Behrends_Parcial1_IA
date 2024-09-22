using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FSM<T ,J> where J : MonoBehaviour
{
    private Dictionary<T, BaseState<T, J>> _posibleStates;
    private BaseState<T, J> _actualState;

    public Dictionary<T, BaseState<T, J>> PosibleStates
    {
        get {return _posibleStates;}
    }

    public FSM()
    {
        _posibleStates = new Dictionary<T, BaseState<T, J>>();
    }

    public FSM<T,J> AddState(T stateKey, BaseState<T,J> newState)
    {
        _posibleStates.Add(stateKey, newState);
        return this;
    }

    public void SetUpStates(J character)
    {
        foreach(var state in _posibleStates.Values)
        {
            state.SetUp(character, this);
        }
    }

    public void OnUpdate()
    {
        _actualState?.OnUpdate();
    }

    public void ChangeState(T stateToChange)
    {
        _actualState?.OnExit();
        _actualState = _posibleStates[stateToChange];
        _actualState.OnEnter();
    }
}
