using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private LayerMask _flockMask;
    [SerializeField] public Transform target;
    [SerializeField] private float _speed;
    [SerializeField] public float stamina;
    [SerializeField] private float _maxStamina = 10f;
    [SerializeField] public float restTime = 0f;
    [SerializeField] private float _restToContinue = 3f;


    public FSM<IAStates, EnemyIA> fsm;

    #region getters y setters
    public float Speed
    {
        get
        {
            return _speed;
        }
    }

    public float MaxStamina
    {
        get { return _maxStamina; }
    }

    public float RestToContinue
    {
        get { return _restToContinue; }
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
        stamina = _maxStamina;
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

    public void Move(Vector3 dir)
    {
        transform.position += dir.normalized * _speed * Time.deltaTime;
        stamina -= Time.deltaTime;
    }
}
