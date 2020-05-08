using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    //controller

    public Waypoint currentWaypoint;
    private Vector3 _destination;
    private bool _reachedDestination;

    private float _speed;
    private int _direction;

    void Start()
    {
        SetDestination(currentWaypoint.GetPosition());
        _speed = Random.Range(0.8f, 1.3f);
        float _random = Random.Range(0, 2);
        _direction = Mathf.RoundToInt(_random);
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
        //this.transform.LookAt(_destination);
        this.transform.position = Vector3.MoveTowards(this.transform.position, _destination, Time.deltaTime * _speed);
    }

    private void SetDestination(Vector3 destination)
    {
        _destination = destination;
        Debug.Log("_destination : " + _destination);
        transform.LookAt(currentWaypoint.transform);
    }
}
