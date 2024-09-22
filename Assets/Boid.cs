using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private Transform _target;
    private Transform _danger;

    private Vector3 _fleeVector;
    private Vector3 _seekVector;

    private float _overlapRadius = 10; 

    [Header("Weights")]
    [Range(0,1)] [SerializeField] private float _fleeWeight;
    [Range(0,1)] [SerializeField] private float _seekWeight;
    [Range(0,1)] [SerializeField] private float _separationWeight;
    [Range(0,1)] [SerializeField] private float _cohesionWeight;
    [Range(0,1)] [SerializeField] private float _allignWeight;

    private GameObject[] _closeBoids;

    private void Awake()
    {
        RandomizeFoward();
    }

    private void Update()
    {
        GetCloseBoids();
        CalculateAndMove();
        BoidManager.Instance.CheckBounds(this);
    }


    private void CalculateAndMove()
    {
        transform.forward += CalculateDir();
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private Vector3 CalculateDir()
    {
        var dir = Separation().normalized * _separationWeight + Cohesion().normalized * _cohesionWeight + Allign().normalized * _allignWeight;
        return dir;
    }

    private GameObject[] GetCloseBoids()
    {
        _closeBoids = Physics.OverlapSphere(transform.position, _overlapRadius).Select(x => x.gameObject).ToArray();


        return _closeBoids;
    }

    public void Seek()
    {
        var dir = _target.position - _danger.position;
        dir.y = 0;
        _seekVector = transform.forward * (1-_rotationSpeed) + dir * _rotationSpeed;
    }

    public void Flee()
    {
        var dir = transform.position - _danger.position;  
        dir.y = 0;
        _fleeVector = transform.forward * (1 - _rotationSpeed) + dir * _rotationSpeed;
    }

    public Vector3 Cohesion()
    {
        var dir = Vector3.zero;
        foreach (var i in _closeBoids) 
        {
            dir += i.transform.position;
        }
        dir.y = 0;
        return dir;
    }

    public Vector3 Separation()
    {
        var dir = Vector3.zero;
        foreach(var i in _closeBoids)
        {
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

    private void RandomizeFoward()
    {
        var z = Random.Range(-1f, 1f);
        var x = Random.Range(-1f, 1f);

        transform.forward = new Vector3 (x, 0f, z);
    }
}
