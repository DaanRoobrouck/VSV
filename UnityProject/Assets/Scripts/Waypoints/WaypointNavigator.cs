using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    //controller

    public Waypoint currentWaypoint;
    private Vector3 _destination;
    private bool _reachedDestination;

    private float _speed;
    private int _direction;

    private void Awake()
    {
        //assign controller
    }

    void Start()
    {
        SetDestination(currentWaypoint.GetPosition());
        _speed = Random.Range(0.8f, 1.3f);
        float _random = Random.Range(0, 2);
        Debug.Log("r: " + _random);
        _direction = Mathf.RoundToInt(_random);
        Debug.Log("d: " + _direction);
    }

    void Update()
    {
        float distanceToWaypoint = Vector3.Distance(this.transform.position, _destination);
        if (distanceToWaypoint <= 0.5f)
        {
            if (_direction == 0)
            {
                currentWaypoint = currentWaypoint.NextWaypoint;
            }
            else
            {
                currentWaypoint = currentWaypoint.PreviousWaypoint;
            }
            
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
