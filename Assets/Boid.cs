using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private LayerMask _foodMask;
    [SerializeField] private LayerMask _dangerMask;

    private Transform _target;
    private Transform _danger;

    private Vector3 _fleeVector;
    private Vector3 _seekVector;

    [SerializeField] private float _overlapRadius = 10; 

    [Header("Weights")]
    [Range(0,1)] [SerializeField] private float _fleeWeight;
    [Range(0,1)] [SerializeField] private float _seekWeight;
    [Range(0,1)] [SerializeField] private float _separationWeight;
    [Range(0,1)] [SerializeField] private float _cohesionWeight;
    [Range(0,1)] [SerializeField] private float _allignWeight;

    private GameObject[] _closeBoids;
    private GameObject[] _closeFood;

    private void Awake()
    {
        RandomizeFoward();
    }

    private void Update()
    {
        GetCloseBoids();
        _danger = GetDanger();
        CalculateAndMove();
        BoidManager.Instance.CheckBounds(this);
    }


    private void CalculateAndMove()
    {
        transform.forward = CalculateDir();
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private Vector3 CalculateDir()
    {
        GetCloseFood();
        var dir=Vector3.zero;
        dir = Separation().normalized * _separationWeight +
            Cohesion().normalized * _cohesionWeight +
            Allign().normalized * _allignWeight +
            Seek().normalized * _seekWeight + 
            Flee() * _fleeWeight;

        /*
        if (_closeFood.Length == 0)
        {
            dir = Separation().normalized * _separationWeight + Cohesion().normalized * _cohesionWeight + Allign().normalized * _allignWeight;
            print("imehere");
        }
        else
        {
            _target = _closeFood[0].transform;
            dir = _closeFood[0].transform.position - transform.position;
            
        }*/
        return dir;
    }
    
    private GameObject[] GetCloseBoids()
    {
        _closeBoids = Physics.OverlapSphere(transform.position, _overlapRadius).Select(x => x.gameObject).ToArray();
        Debug.Log("CLOSE BOIDS: " + _closeBoids.Length);
        return _closeBoids;
    }

    private Transform GetDanger()
    {

        var danger = Physics.OverlapSphere(transform.position, _overlapRadius, _dangerMask).Select(x => x.transform).ToArray();
        if (danger.Length > 0)
        {
            return danger[0].transform;
        }
        return null;
    }

    private GameObject[] GetCloseFood()
    {
        
        _closeFood = Physics.OverlapSphere(transform.position, _overlapRadius, _foodMask).Select(x => x.gameObject).ToArray();

        print(_closeFood.Length);

        return _closeFood;
    }


    public Vector3 Seek()
    {
        Vector3 dangerPos = Vector3.zero;
        var dir = Vector3.zero;
        if(_danger != null)
        {
            dangerPos = _danger.position;
        }
        if(_closeFood.Length > 0)
        {
            _target = _closeFood[0].transform;
            dir +=  _target.position - transform.position - dangerPos;
            if (dir.magnitude < 0.1f)
            {
                Destroy(_closeFood[0].gameObject);
            }
        }

        
        dir.y = 0;
        //_seekVector = transform.forward * (1-_rotationSpeed) + dir * _rotationSpeed;
        return dir;
    }

    public Vector3 Flee()
    {
        var dir = Vector3.zero;
        if (_danger != null)
        {
            dir = transform.position - _danger.position;
            dir.y = 0;
        } 
            //_fleeVector = transform.forward * (1 - _rotationSpeed) + dir * _rotationSpeed;
        return dir;
    }

    public Vector3 Cohesion()
    {
        var dir = Vector3.zero;
        foreach (var i in _closeBoids) 
        {
            dir += i.transform.position - transform.position;
        }
        dir.y = 0;
        return dir;
    }

    public Vector3 Separation()
    {
        var dir = Vector3.zero;
        foreach(var i in _closeBoids)
        {
            Debug.Log("SEPARATION close boids");
            dir += transform.position - i.transform.position; 
        }

        dir.y = 0;
        return dir;
    }

    public Vector3 Allign()
    {
        var dir = Vector3.zero;
        foreach(var i in _closeBoids)
        {
            dir += i.transform.forward;
        }
        dir.y = 0;
        return dir;
    }

    public Vector3 Evade()
    {
        var dir = Vector3.zero;
        if(_danger != null)
            dir += transform.position - _danger.transform.position;
        dir.y = 0;
        return dir;
    }

    private void RandomizeFoward()
    {
        var z = Random.Range(-1f, 1f);
        var x = Random.Range(-1f, 1f);

        transform.forward = new Vector3 (x, 0f, z);
    }
}
