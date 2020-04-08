using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarNavigator : MonoBehaviour
{
    //controller

    public CarWaypoint currentWaypoint;
    private Vector3 _destination;
    private bool _reachedDestination;

    [Range(3f,5f)][SerializeField] private float _speed;

    private void Awake()
    {
        //assign controller
    }

    void Start()
    {
        SetDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        float distanceToWaypoint = Vector3.Distance(this.transform.position, _destination);
        if (distanceToWaypoint <= 0.5f)
        {
            currentWaypoint = currentWaypoint.PreviousWaypoint;

                SetDestination(currentWaypoint.GetPosition());
        }
        this.transform.LookAt(_destination);
        this.transform.position = Vector3.MoveTowards(this.transform.position, _destination, Time.deltaTime * _speed);
    }

    private void SetDestination(Vector3 destination)
    {
        _destination = destination;
        transform.LookAt(currentWaypoint.transform);
        //_reachedDestination = false;
    }
}