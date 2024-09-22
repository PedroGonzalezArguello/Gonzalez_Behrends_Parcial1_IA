using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private LayerMask _flockMask;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    public FSM<IAStates, EnemyIA> fsm;

    #region getters y setters
    public float Speed
    {
        get
        {
            return _speed;
        }
    }
    public Transform Target
    {
        get
        {
            return _target;
        }
    }

    public LayerMask FlockMask
    {
        get
        {
            return _flockMask;
        }
    }

    public List<Transform> Waypoints
    {
        get
        {
            return _waypoints;
        }
    }
    #endregion
    public enum IAStates
    {
        REST,
        PATROL,
        CHASE
    }

    private void Awake()
    {
        fsm = new FSM<IAStates, EnemyIA>()
            .AddState(IAStates.PATROL, new Patrol())
            .AddState(IAStates.REST, new Rest())
            .AddState(IAStates.CHASE, new Chase());

        fsm.SetUpStates(this);
        fsm.ChangeState(IAStates.PATROL);
    }

    private void Update()
    {
        fsm.OnUpdate();
    }
}
