﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using Assets.Scripts.UI;


public class DestinationManager : MonoBehaviour
{
    [SerializeField] private BoxCollider[] _destinations;
    public BoxCollider[] CurrentDestinations { get; set; }
    public Transform CurrentDestination { get; set; }
    private int _currentDestinationIndex = 0; 

    private Random _random;

    public event EventHandler<DestinationEventArgs> UpdateDestination;
    [SerializeField] private UIManager _uiListener;


    // Start is called before the first frame update
    void Start()
    {
        this.UpdateDestination += (sender, args) => _uiListener.UpdateText(CurrentDestination);
        _random = new Random();
        GetRandomDestinations(_destinations, 3);
        SetDestination();
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
        UpdateDestination?.Invoke(this, new DestinationEventArgs(CurrentDestination) );
        //event to update UI (also call it when updating the destination)
    }
}