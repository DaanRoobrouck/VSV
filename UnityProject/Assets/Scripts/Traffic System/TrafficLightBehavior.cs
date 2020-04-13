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

    private TrafficLightColor _lightState;

    [SerializeField] private bool _hasPedestrianLight;
    private bool _buttonPressed;

    
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
            if (Input.GetButtonDown("Interact") && !_buttonPressed)
            {
                _buttonPressed = true;
                Debug.Log("pressed");
                StartCoroutine(CallRedLight(5));
            }
        }
    }

    private IEnumerator GreenLight()
    {
        if (!_buttonPressed)
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
        if (!_buttonPressed)
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
        if (!_buttonPressed)
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

    private IEnumerator CallRedLight(int timer)
    {
        yield return new WaitForSeconds(timer);
        StartCoroutine(CalledOrangeLight());
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
        _buttonPressed = false;
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
}
