using System.Net.Configuration;
using UnityEngine;

public class CarNavigator : MonoBehaviour
{
    //controller

    public CarWaypoint currentWaypoint;
    private Vector3 _destination;
    private bool _reachedDestination;


    public bool CanDrive { get; set; } = true;


    [Range(3f,10f)][SerializeField] private float _speed;

    [SerializeField] private GameObject _carBefore;
    [SerializeField] private GameObject _carAfter;

    [SerializeField] private bool _stationary;

  

    void Start()
    {
        SetDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {

        if (CanDrive && !_stationary)
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
    }

    private void SetDestination(Vector3 destination)
    {
        _destination = destination;
        transform.LookAt(currentWaypoint.transform);
        //_reachedDestination = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_stationary)
        {
          if (other.CompareTag("Light"))
          {
             TrafficLightColor _lightState = other.GetComponent<TrafficLightBehavior>().LightState;
              if (_lightState == TrafficLightColor.Red)
              {
                  CanDrive = false;
              }
              else
              {
                  CanDrive = true;
              }
          }

          if (other.CompareTag("Vehicle"))
          {
              if (other.gameObject == _carAfter)
              {
                  other.GetComponent<CarNavigator>().CanDrive = false;
              }
              else if (other.gameObject == _carBefore)
              {
                  CanDrive = false;

              }
          }
          }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            other.GetComponent<CarNavigator>().CanDrive = true;
        }
    }
}