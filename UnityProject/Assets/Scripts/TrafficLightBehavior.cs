using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightBehavior : MonoBehaviour
{
    public enum Light {Red, Green, Orange}
    public Material MaterialRedLight;
    public Material MaterialGreenLight;
    public Material MaterialOrangeLight;
    public Material MaterialBlackLight;
    public Light currentLight;

    private float _time;
    private bool _isOrange,_isRed = false;
    private bool _isGreen = true;
    [SerializeField]private float _redTimer;
    [SerializeField]private float _orangeTimer;
    [SerializeField]private float _greenTimer;
    [SerializeField]private MeshRenderer _renderer;
    [SerializeField]private Material[] _currentLightMaterials = new Material[5];

    // Start is called before the first frame update
    void Start()
    {
        _currentLightMaterials[2] = MaterialGreenLight;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isOrange)
        {
            currentLight = Light.Orange;
            _time += Time.deltaTime;
            if(_time>_orangeTimer)
            {
            Debug.Log("Red");
            _currentLightMaterials[3] = MaterialBlackLight;
            _currentLightMaterials[4] = MaterialBlackLight;
            _currentLightMaterials[2]= MaterialRedLight;
            _renderer.materials = _currentLightMaterials;
            _isRed = true;
            _isOrange = false;
            _time =0;
            }
        }
        if(_isRed)
        {
            currentLight = Light.Red;
            _time += Time.deltaTime;
            if(_time>_redTimer)
            {
            Debug.Log("Green");
            _currentLightMaterials[2] = MaterialBlackLight;
            _currentLightMaterials[3] = MaterialBlackLight;
            _currentLightMaterials[4]= MaterialGreenLight;
            _renderer.materials = _currentLightMaterials;
            _isRed = false;
            _isGreen = true;
            _time =0;
            }
        }
        if(_isGreen)
        {
            currentLight = Light.Green;
            _time += Time.deltaTime;
            if(_time>_greenTimer)
            {
            Debug.Log("Orange");
            _currentLightMaterials[4] = MaterialBlackLight;
            _currentLightMaterials[2] = MaterialBlackLight;
            _currentLightMaterials[3]= MaterialOrangeLight;
            _renderer.materials = _currentLightMaterials;
            _isGreen = false;
            _isOrange = true;
            _time =0;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // if(other.tag =="Player")
        // {
        //     _currentLightMaterials[4] = MaterialBlackLight;
        //     _currentLightMaterials[3] = MaterialOrangeLight;
        //     _currentLightMaterials[2] = MaterialBlackLight;
        //     _isOrange = true;
        //     _renderer.materials = _currentLightMaterials;
        // }
    }
}
