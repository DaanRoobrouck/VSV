using UnityEngine;

public class CarNavigator : MonoBehaviour
{
    //controller

    public CarWaypoint currentWaypoint;
    private Vector3 _destination;
    private bool _reachedDestination;


    public bool CanDrive { get; set; } = true;


    [Range(3f,10f)][SerializeField] private float _speed;

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

        if (CanDrive)
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

        }
    }
}