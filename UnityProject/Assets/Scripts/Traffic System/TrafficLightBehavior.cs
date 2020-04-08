using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficLightColor
{
    Red,
    Orange,
    Green
}

public enum TrafficLightRotation
{
    A, B
}


public class TrafficLightBehavior : MonoBehaviour
{

    [Header("Traffic Light Configuration")]
    [SerializeField] private Material[] _lightMaterials;
    [Tooltip("We decide which traffic lights are parallel with each other so they will have the same lights lit up. ")]
    [SerializeField] private TrafficLightRotation _lightRotation;

    [SerializeField] private bool _hasPedestrianLight;

    
    [Range(5f,25f)][SerializeField]private int _redAndGreenTimer;
    [Range(2f,5f)][SerializeField]private int _orangeTimer;
    private MeshRenderer _renderer;

    private int _blackLightIndex = 1;
    private int _redLightIndex = 2;
    private int _orangeLightIndex = 3;
    private int _greenLightIndex = 4;
    [Tooltip("Gets assigned at runtime")]
    [SerializeField] private Material[] _currentLightMaterials;

    void Start()
    {
        _renderer = this.GetComponent<MeshRenderer>();
        _currentLightMaterials = _renderer.sharedMaterials;
        switch (_lightRotation)
        {
            case TrafficLightRotation.A:
                StartCoroutine(RedLight());
                break;
            case TrafficLightRotation.B:
                StartCoroutine(GreenLight());
                break;
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
       // if(other.tag =="Player")
       // {
       ////   _currentLightMaterials[4] = MaterialBlackLight;
       ////   _currentLightMaterials[3] = MaterialOrangeLight;
       ////   _currentLightMaterials[2] = MaterialBlackLight;
       //     _isOrange = true;
       //     _renderer.materials = _currentLightMaterials;
       // }
    }

    private IEnumerator GreenLight()
    {
        _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
        _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_blackLightIndex];
        _currentLightMaterials[_greenLightIndex] = _lightMaterials[_greenLightIndex];
        _renderer.sharedMaterials = _currentLightMaterials;

        yield return new WaitForSeconds(_redAndGreenTimer - _orangeTimer);
        StartCoroutine(OrangeLight());
    }
    private IEnumerator OrangeLight()
    {
        _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
        _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_orangeLightIndex];
        _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
        _renderer.sharedMaterials = _currentLightMaterials;
        yield return new WaitForSeconds(_orangeTimer);
        StartCoroutine(RedLight());
    }
    private IEnumerator RedLight()
    {
        _currentLightMaterials[_redLightIndex] = _lightMaterials[_redLightIndex];
        _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_blackLightIndex];
        _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
        _renderer.sharedMaterials = _currentLightMaterials;
        yield return new WaitForSeconds(_redAndGreenTimer);
        StartCoroutine(GreenLight());
    }
}
