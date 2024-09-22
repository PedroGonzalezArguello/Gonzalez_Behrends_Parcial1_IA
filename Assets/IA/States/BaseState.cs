using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T, J> where J : MonoBehaviour
{
    protected J _character;
    protected FSM<T, J> _fsm;

    public void SetUp(J chartacter, FSM<T, J> fsm)
    {
        _character = chartacter;
        _fsm = fsm;
    }

    public virtual void OnUpdate() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}
