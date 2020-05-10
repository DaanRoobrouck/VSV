using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class DestinationManager : MonoBehaviour
{
    [SerializeField] private BoxCollider[] _destinations;
    public BoxCollider[] CurrentDestinations { get; set; }
    public Transform CurrentDestination { get; set; }
    public int CurrentDestinationIndex = 0; 

    private Random _random;

    public event EventHandler<DestinationEventArgs> UpdateDestination;
    [SerializeField] private UIManager _uiListener;

    [SerializeField] private GameObject[] _trafficSigns;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _normalMaterial;


    // Start is called before the first frame update
    void Start()
    {
        _trafficSigns = GameObject.FindGameObjectsWithTag("TrafficSign");
        this.UpdateDestination += (sender, args) => _uiListener.UpdateDestinationText(CurrentDestination);
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
    }

    public void SetDestination()
    {
        if (CurrentDestinationIndex == CurrentDestinations.Length)
        {
            SceneManager.LoadScene("ScoreDisplay");
        }
        CurrentDestination = CurrentDestinations[CurrentDestinationIndex].transform;
        CurrentDestination.GetComponent<BoxCollider>().enabled = true;
        UpdateDestination?.Invoke(this, new DestinationEventArgs(CurrentDestination) );
        foreach(GameObject sign in _trafficSigns)
        {
            string text = sign.GetComponentInChildren<TextMeshPro>().text;
            if(text ==CurrentDestination.name.ToString())
            {
                Debug.Log("Sign Gevonden");
                sign.GetComponent<MeshRenderer>().materials[1]=_highlightMaterial;
            }
            else sign.GetComponent<MeshRenderer>().materials[1]=_normalMaterial;
        }
        //event to update UI (also call it when updating the destination)
    }
}
