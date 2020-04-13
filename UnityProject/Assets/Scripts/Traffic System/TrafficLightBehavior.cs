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

    private TrafficLightColor _lightState;

    [SerializeField] private bool _hasPedestrianLight;
    public bool ButtonPressed;

    
    [Range(5f,25f)][SerializeField]private int _redAndGreenTimer;
    [Range(2f,5f)][SerializeField]private int _orangeTimer;
    private MeshRenderer _renderer;

    private int _blackLightIndex = 1;
    private int _redLightIndex = 2;
    private int _orangeLightIndex = 3;
    private int _greenLightIndex = 4;
    [Tooltip("Gets assigned at runtime")]
    [SerializeField] private Material[] _currentLightMaterials;

    [SerializeField]
    private TrafficLightBehavior[] _sameCrossRoadLights;

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
           if (_lightState == TrafficLightColor.Red)
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

        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact") && !ButtonPressed)
            {
                Debug.Log("pressed");
                foreach (var trafficLight in _sameCrossRoadLights)
                {
                    trafficLight.ButtonPressed = true;
                    if (trafficLight.LightRotation == this.LightRotation)
                    {
                         StartCoroutine(CallLight(5, trafficLight.CalledOrangeLight()));
                    }
                    else
                    {
                        StartCoroutine(CallLight(8, trafficLight.CalledGreenLight()));

                    }
                }
            }
        }
    }

    private IEnumerator GreenLight()
    {
        if (!ButtonPressed)
        {
          _lightState = TrafficLightColor.Green;
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
        if (!ButtonPressed)
        {
          _lightState = TrafficLightColor.Orange;
          _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
          _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_orangeLightIndex];
          _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
          _renderer.sharedMaterials = _currentLightMaterials;
          yield return new WaitForSeconds(_orangeTimer);
          StartCoroutine(RedLight());
        }
    }
    private IEnumerator RedLight()
    {
        if (!ButtonPressed)
        {
         _lightState = TrafficLightColor.Red;
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

    private IEnumerator CallLight(int timer, IEnumerator enumerator)
    {
        yield return new WaitForSeconds(timer);
        StartCoroutine(enumerator);
    }

    private IEnumerator CalledRedLight()
    {
        _lightState = TrafficLightColor.Red;
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
        ButtonPressed = false;
        StartCoroutine(GreenLight());
    }

    private IEnumerator CalledOrangeLight()
    {
      
       _lightState = TrafficLightColor.Orange;
       _currentLightMaterials[_redLightIndex] = _lightMaterials[_blackLightIndex];
       _currentLightMaterials[_orangeLightIndex] = _lightMaterials[_orangeLightIndex];
       _currentLightMaterials[_greenLightIndex] = _lightMaterials[_blackLightIndex];
       _renderer.sharedMaterials = _currentLightMaterials;
       yield return new WaitForSeconds(_orangeTimer);
       StartCoroutine(CalledRedLight());
    }

    private IEnumerator CalledGreenLight()
    {
       
            _lightState = TrafficLightColor.Green;
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
            ButtonPressed = false;
            StartCoroutine(OrangeLight());
    }
}
