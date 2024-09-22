using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Chase : BaseState<EnemyIA.IAStates, EnemyIA>
{
    public override void OnUpdate()
    {
        if(_character.stamina <= 0)
        {
            _fsm.ChangeState(EnemyIA.IAStates.REST);
        }
        var dir = _character.target.transform.position - _character.transform.position;
        dir.y = 0;
        _character.transform.forward = (_character.transform.forward * 0.9f + dir * 0.1f);
        if(dir.magnitude < 1) return;
        _character.Move(dir);
    }
}
