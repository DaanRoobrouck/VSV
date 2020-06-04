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
    public TrafficLightRotation LightRotation;

    public TrafficLightColor LightState;

    [SerializeField] private bool _hasPedestrianLight;
    [SerializeField] private bool _isOnlyPedestrianLight;

    
    [Range(5f,25f)][SerializeField]private int _redAndGreenTimer;
    [Range(2f,5f)][SerializeField]private int _orangeTimer;
    private MeshRenderer _renderer;

    private int _blackLightIndex = 1;
    private int _redLightIndex = 2;
    private int _orangeLightIndex = 3;
    private int _greenLightIndex = 4;

    [Tooltip("Gets assigned at runtime")]
    [SerializeField] private Material[] _currentLightMaterials;

    
    public TrafficLightBehavior[] SameCrossRoadLights;

    void Start()
    {
        _renderer = this.GetComponent<MeshRenderer>();
        _currentLightMaterials = _renderer.sharedMaterials;
        switch (LightRotation)
        {
            case TrafficLightRotation.A:
                StartCoroutine(RedLight());
                break;
            case TrafficLightRotation.B:
                StartCoroutine(GreenLight());
                break;
        }
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            if (!_isOnlyPedestrianLight)
            {
               if (LightState == TrafficLightColor.Red)
               {
                    if (other.GetComponent<CarNavigator>() != null)
                    {
                        other.GetComponent<CarNavigator>().CanDrive = false;
                    }
               }
               else
               {
                   other.GetComponent<CarNavigator>().CanDrive = true;
               }
            }
        }
    }

    private IEnumerator GreenLight()
    {
        
              LightState = TrafficLightColor.Green;
              if (_isOnlyPedestrianLight) 
              {

                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(1).gameObject.SetActive(false);
                _currentLightMaterials[4] = _lightMaterials[0];
                    _currentLightMaterials[5] = _lightMaterials[2];
                    _renderer.sharedMaterials = _currentLightMaterials;
                    yield return new WaitForSeconds(_redAndGreenTimer);
                    StartCoroutine(RedLight());
                
              }
              else
              {
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(true);

                _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
                _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_blackLightIndex];
                _currentLightMaterials[_greenLightIndex] = _lightMaterials[_greenLightIndex];

              if (_hasPedestrianLight)
              {
                  _currentLightMaterials[7] = _lightMaterials[_redLightIndex];
                  _currentLightMaterials[8] = _lightMaterials[_blackLightIndex];
              }
              _renderer.sharedMaterials = _currentLightMaterials;

              yield return new WaitForSeconds(_redAndGreenTimer - _orangeTimer);
              StartCoroutine(OrangeLight());
            }
    }
    private IEnumerator OrangeLight()
    {
       
          LightState = TrafficLightColor.Orange;
          _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
          _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_orangeLightIndex];
          _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
          _renderer.sharedMaterials = _currentLightMaterials;
          yield return new WaitForSeconds(_orangeTimer);
          StartCoroutine(RedLight());
    }
    private IEnumerator RedLight()
    {
            LightState = TrafficLightColor.Red;

        if (_isOnlyPedestrianLight)
        {
            
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(true);
                _currentLightMaterials[4] = _lightMaterials[1];
            _currentLightMaterials[5] = _lightMaterials[0];
            _renderer.sharedMaterials = _currentLightMaterials;
            yield return new WaitForSeconds(_redAndGreenTimer);
            StartCoroutine(GreenLight());
        }
        else
        {
               this.transform.GetChild(0).gameObject.SetActive(true);
               this.transform.GetChild(1).gameObject.SetActive(false);
                _currentLightMaterials[_redLightIndex] = _lightMaterials[_redLightIndex];
            _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_blackLightIndex];
            _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
          
            if (_hasPedestrianLight)
            {
                _currentLightMaterials[7] = _lightMaterials[_blackLightIndex];
                _currentLightMaterials[8] = _lightMaterials[_greenLightIndex];
            }
          
            _renderer.sharedMaterials = _currentLightMaterials;
            yield return new WaitForSeconds(_redAndGreenTimer);
            StartCoroutine(GreenLight());
        }
    }

  
}
