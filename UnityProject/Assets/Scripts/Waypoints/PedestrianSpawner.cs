using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _pedestriansToSpawn;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while (count < _pedestriansToSpawn)
        {
            GameObject go = Instantiate(_prefab);
            Transform child = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount - 1));
            go.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            go.transform.position = child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
