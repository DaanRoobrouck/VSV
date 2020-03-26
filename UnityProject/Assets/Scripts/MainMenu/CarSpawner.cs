using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject SportsCar;
    public GameObject Truck;
    public GameObject Bike;
    public GameObject IcecreamTruck;
    public GameObject RedCar;


    public Transform SpawnPoint;


    private float spawnTimer;
    private float despawnTimer;


    private GameObject currentCar;
    private int carNumber;
    private void Start()
    {
        SpawnPoint = this.gameObject.transform;
        SelectCar();
        SpawnCar();
    }
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= 4)
        {
            SelectCar();
            SpawnCar();
            spawnTimer = 0;
        }
    }

    private void SelectCar()
    {
        carNumber = UnityEngine.Random.Range(0, 5);
        switch (carNumber)
        {
            case 0:
                currentCar = SportsCar;
                break;
            case 1:
                currentCar = Truck;
                break;
            case 2:
                currentCar = Bike;
                break;
            case 3:
                currentCar = IcecreamTruck;
                break;
            case 4:
                currentCar = RedCar;
                break;
        }
    }

    private void SpawnCar()
    {
        Instantiate(currentCar, SpawnPoint);
    }
}
