using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBehaviour : MonoBehaviour
{ 
    private DestinationManager _destinationManager;

    private void Start()
    {
        _destinationManager = this.transform.parent.GetComponent<DestinationManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          this.GetComponent<BoxCollider>().enabled = false;
          _destinationManager.CurrentDestinationIndex++;
          _destinationManager.SetDestination();
        }
    }
}
