using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;


public class DestinationManager : MonoBehaviour
{
    [SerializeField] private BoxCollider[] _destinations;
    public BoxCollider[] CurrentDestinations { get; set; }
    public Transform CurrentDestination { get; set; }
    private int _currentDestinationIndex = 0; 

    private Random _random;


    // Start is called before the first frame update
    void Start()
    {
        _random = new Random();
        GetRandomDestinations(_destinations, 3);
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetRandomDestinations(BoxCollider[] destinations, int amountOfDestinations)
    {
        CurrentDestinations = new BoxCollider[3];
        for (int i = 0; i < amountOfDestinations; i++)
        {
            int index = _random.Next(0, _destinations.Length);

            while (CurrentDestinations.Contains(_destinations[index]))
            {
                index = _random.Next(0, _destinations.Length);
            }
            CurrentDestinations[i] = _destinations[index];
        }
        Debug.Log(CurrentDestinations);
    }

    private void SetDestination()
    {
        CurrentDestination = CurrentDestinations[_currentDestinationIndex].transform;
        //event to update UI (also call it when updating the destination)
    }
}
