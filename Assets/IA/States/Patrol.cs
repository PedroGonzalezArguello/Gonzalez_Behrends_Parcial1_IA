using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState<EnemyIA.IAStates, EnemyIA>
{
    private int _wayCount;
    private bool _isWaiting;
    private float _waitForTime = 1, _timer;
    private float _viewAngle = 45f;


    public override void OnUpdate()
    {
        var target = Physics.OverlapSphere(_character.transform.position, 5, _flockMask);

        if(target.Length > 0)
        {
            var targetDir = (target[0].transform.position - _character.transform.position);
            if(Vector3.Angle(_character.transform.forward, targetDir) < _viewAngle / 2)
            {

            }
        }

        var actualDir = (_character.Waypoints[_wayCount].position - _character.transform.position);
        actualDir.y = 0;

        _character.transform.forward = (_character.transform.forward * 0.99f + actualDir.normalized * 0.01f);

        if (_isWaiting)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _isWaiting = false;
            }
            return;
        }

        _character.transform.position += actualDir.normalized * Time.deltaTime * _character.Speed;

        if(actualDir.magnitude < 0.2f)
        {
            _wayCount++;
            if (_wayCount >= _character.Waypoints.Count) 
            {
                _wayCount = 0;
            }
            _isWaiting = true;
            _timer = _waitForTime;
        }
    }
}
