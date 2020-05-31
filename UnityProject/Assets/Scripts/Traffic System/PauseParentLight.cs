using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseParentLight : MonoBehaviour
{
    [SerializeField] private TrafficLightBehavior _parentLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var light in _parentLight.SameCrossRoadLights)
            {
                light.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var light in _parentLight.SameCrossRoadLights)
            {
                light.enabled = true;
            }
        }
    }

   
}
