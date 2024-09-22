using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : BaseState<EnemyIA.IAStates, EnemyIA>
{
    public override void OnUpdate()
    {
        if (_character.restTime < _character.RestToContinue)
        {
            _character.restTime += Time.deltaTime;
        } else
        {
            _character.restTime = 0;
            _character.stamina = _character.MaxStamina;
            _fsm.ChangeState(EnemyIA.IAStates.PATROL);
        }
    }
}
