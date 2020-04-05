using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayScript : MonoBehaviour
    {
    [SerializeField] private Vector3 _amplitude;
    [SerializeField] private Vector3 _rotAmplitude;
    [SerializeField] private Vector3 _frequency;
    [SerializeField] private Vector3 _rotFrequency;
    [SerializeField] private float _offset;
    public float Scale { get; set; }
    public float RotScale { get; set; }

    private Vector3 _startPosition;
    private Vector3 _startRotation;

    // Start is called before the first frame update
    void Start()
        {
        Scale = 1;
        RotScale = 1;
        _startPosition = transform.localPosition;
        _startRotation = transform.localEulerAngles;
        }

    // Update is called once per frame
    void Update()
        {
        float offsetFramecount = Time.time + _offset;
        transform.localPosition = _startPosition + new Vector3(_amplitude.x * Mathf.Sin(offsetFramecount * _frequency.x), _amplitude.y * Mathf.Sin(offsetFramecount * _frequency.y), _amplitude.z * Mathf.Sin(offsetFramecount * _frequency.z)) * Scale;
        transform.localEulerAngles = _startRotation + new Vector3(_rotAmplitude.x * Mathf.Sin(offsetFramecount * _rotFrequency.x), _rotAmplitude.y * Mathf.Sin(offsetFramecount * _rotFrequency.y), _rotAmplitude.z * Mathf.Sin(offsetFramecount * _rotFrequency.z)) * RotScale;
        }
    }
