using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SituationController : MonoBehaviour
{
    private CardController _cardController;
    private FirstPersonAIO _fpsController;
    private Situation _situation;

    public Situation Situation { get => _situation; set => _situation = value; }

    private bool _hasStopped = false;
    private bool _hasLooked = false;
    private bool _hasLookedCars = false;
    private bool _checkDistance = false;

    private bool _lookLeft = true;
    private bool _lookRight = false;
    private bool _lookForward = false;
    private bool _lookBackward = false;
    private int _leftCount = 0;
    private int _rightCount = 0;
    private int _forwardCount = 0;
    private int _backwardCount = 0;

    private int _tries = 0;
    public int Tries { get => _tries; set => _tries = value; }

    public bool CorrectSequence = false;

    [SerializeField] private float _stopTime;
    [SerializeField] private int _aantalKeerKijken = 2;

    public LayerMask LayerMask;

    [SerializeField] private GameObject _lookGO;
    [SerializeField] private GameObject _leftLookGO;
    [SerializeField] private GameObject _rightLookGO;
    [SerializeField] private GameObject _forwardLookGO;
    [SerializeField] private GameObject _backwardLookGO;
    [SerializeField] private GameObject _leftCarLookGO;
    [SerializeField] private GameObject _rightCarLookGO;
    [SerializeField] private GameObject _checkBoundaryGO;
    private GameObject _distanceGO;
    private GameObject _endGO;

    private ScoreManager _scoreManager;
    private float _timer;
    private float _stopTimer;

    void Start()
    {
        _cardController = this.GetComponent<CardController>();
        _fpsController = FindObjectOfType<FirstPersonAIO>();
        _distanceGO = transform.GetChild(0).gameObject;
        _endGO = transform.GetChild(1).gameObject;
        _situation = _cardController.AssignedSituation;
        _scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));

        if (_checkBoundaryGO != null)
        {
            DeactivateCheck(_checkBoundaryGO);
        }
        if (_endGO != null)
        {
            DeactivateCheck(_endGO);
        }
        if (_distanceGO != null)
        {
            DeactivateCheck(_distanceGO);
        }
        if (_lookGO != null)
        {
            DeactivateCheck(_lookGO);
        }
        if (_leftLookGO != null)
        {
            DeactivateCheck(_leftLookGO);
        }
        if (_rightLookGO != null)
        {
            DeactivateCheck(_rightLookGO);
        }
        if (_forwardLookGO != null)
        {
            DeactivateCheck(_forwardLookGO);
        }
        if (_backwardLookGO != null)
        {
            DeactivateCheck(_backwardLookGO);
        }
    }

    void Update()
    {
        if (CorrectSequence)
        {
            _timer += Time.deltaTime;
            switch (_situation)
            {
                case Situation.PedestrianCrossing:
                    if (!_hasStopped)
                    {
                        if (!_checkBoundaryGO.activeSelf)
                        {
                            ActivateCheck(_checkBoundaryGO);
                        }       
                        CheckStop();
                        return;
                    }
                    if (!_hasLooked)
                    {
                        CheckLookLeftRight();
                        return;
                    }
                    if (!_checkDistance)
                    {
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        if (_checkBoundaryGO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundaryGO);
                        }
                        if (!_endGO.activeSelf)
                        {
                            ActivateCheck(_endGO);
                        }
                        return;
                    }
                    break;
                case Situation.Obstacle:
                    if (!_hasStopped)
                    {
                        if (!_checkBoundaryGO.activeSelf)
                        {
                            ActivateCheck(_checkBoundaryGO);
                        }
                        CheckStop();
                        return;
                    }
                    if (!_hasLooked)
                    {
                        CheckLookOneWay();
                        return;
                    }
                    if (!_checkDistance)
                    {
                        if (_checkBoundaryGO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundaryGO);
                        }
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        if (!_endGO.activeSelf)
                        {
                            ActivateCheck(_endGO);
                        }
                        return;
                    }
                    break;
                case Situation.BetweenCars:
                    if (!_hasStopped)
                    {
                        if (!_checkBoundaryGO.activeSelf)
                        {
                            ActivateCheck(_checkBoundaryGO);
                        }
                        CheckStop();
                        return;
                    }
                    if (!_hasLookedCars)
                    {
                        CheckLookCars();
                        return;
                    }
                    if (!_hasLooked)
                    {
                        CheckLookLeftRight();
                        return;
                    }
                    if (!_checkDistance)
                    {
                        if (_checkBoundaryGO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundaryGO);
                        }
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        if (!_endGO.activeSelf)
                        {
                            ActivateCheck(_endGO);
                        }
                        return;
                    }
                    break;
                case Situation.Crossing:
                    if (!_hasStopped)
                    {
                        if (!_checkBoundaryGO.activeSelf)
                        {
                            ActivateCheck(_checkBoundaryGO);
                        }
                        CheckStop();
                        return;
                    }
                    if (!_hasLooked)
                    {
                        CheckLookFourWay();
                        return;
                    }
                    if (!_checkDistance)
                    {
                        if (_checkBoundaryGO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundaryGO);
                        }
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        if (!_endGO.activeSelf)
                        {
                            ActivateCheck(_endGO);
                        }
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckStop()
    {
        if (_fpsController.fps_Rigidbody.velocity == Vector3.zero)
        {
            _stopTimer += Time.deltaTime;
            if (_stopTimer >= _stopTime)
            {
                Debug.Log("Time up, player stopped correct");
                _hasStopped = true;
                _stopTimer = 0;
            }
        }
    }

    private void CheckLookLeftRight()
    {
        ActivateCheck(_leftLookGO);
        ActivateCheck(_rightLookGO);
        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (_lookLeft && hit.transform.gameObject == _leftLookGO)
            {
                _leftCount++;
                _lookLeft = false;
                _lookRight = true;
                Debug.Log("Looked Left, now look right");
            }
            else if (_lookRight && hit.transform.gameObject == _rightLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookLeft = true;
                Debug.Log("Looked Right, now look left");
            }
        }
        if (_leftCount >= _aantalKeerKijken && _rightCount >= _aantalKeerKijken)
        {
            Debug.Log("Player looked correct");
            DeactivateCheck(_leftLookGO);
            DeactivateCheck(_rightLookGO);
            _hasLooked = true;
            _leftCount = 0;
            _rightCount = 0;
        }
    }

    private void CheckLookCars()
    {
        ActivateCheck(_leftCarLookGO);
        ActivateCheck(_rightCarLookGO);
        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (_lookLeft && hit.transform.gameObject == _leftCarLookGO)
            {
                _leftCount++;
                _lookLeft = false;
                _lookRight = true;
                Debug.Log("Looked at the left car");
            }
            else if (_lookRight && hit.transform.gameObject == _rightCarLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookLeft = true;
                Debug.Log("Looked at the right car");
            }
        }
        if (_leftCount >= 1 && _rightCount >= 1)
        {
            Debug.Log("Player looked correct");
            _hasLookedCars = true;
            _leftCount = 0;
            _rightCount = 0;
        }
    }

    private void CheckLookFourWay()
    {
        ActivateCheck(_leftLookGO);
        ActivateCheck(_rightLookGO);
        ActivateCheck(_forwardLookGO);
        ActivateCheck(_backwardLookGO);
        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (_lookLeft && hit.transform.gameObject == _leftLookGO)
            {
                _leftCount++;
                _lookLeft = false;
                _lookRight = true;
                Debug.Log("Looked Left, now look right");
            }
            else if (_lookRight && hit.transform.gameObject == _rightLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookForward = true;
                Debug.Log("Looked Right, now look forward");
            }
            else if (_lookForward && hit.transform.gameObject == _forwardLookGO)
            {
                _forwardCount++;
                _lookForward = false;
                _lookBackward = true;
                Debug.Log("Looked Forward, now look backward");
            }
            else if (_lookBackward && hit.transform.gameObject == _backwardLookGO)
            {
                _backwardCount++;
                _lookBackward = false;
                _lookLeft = true;
                Debug.Log("Looked Backward, now look left");
            }
        }
        if (_leftCount >= _aantalKeerKijken && _rightCount >= _aantalKeerKijken && _forwardCount >= _aantalKeerKijken && _backwardCount >= _aantalKeerKijken)
        {
            Debug.Log("Player looked correct");
            DeactivateCheck(_leftLookGO);
            DeactivateCheck(_rightLookGO);
            DeactivateCheck(_forwardLookGO);
            DeactivateCheck(_backwardLookGO);
            _hasLooked = true;
            _leftCount = 0;
            _rightCount = 0;
            _forwardCount = 0;
            _backwardCount = 0;
        }
    }

    private void CheckLookOneWay()
    {
        ActivateCheck(_lookGO);

        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (hit.transform.gameObject == _lookGO)
            {
                Debug.Log("Player looked correct");
                DeactivateCheck(_lookGO);
                _hasLooked = true;
            }
        }
    }

    private void ActivateCheck(GameObject go)
    {
        go.SetActive(true);
    }

    private void DeactivateCheck(GameObject go)
    {
        go.SetActive(false);
    }

    public void ResetSituation()
    {
        CorrectSequence = false;

        _scoreManager.AddScore(_timer);

        _hasStopped = false;
        _hasLooked = false;
        _checkDistance = false;

        _lookLeft = true;
        _lookRight = false;
        _leftCount = 0;
        _rightCount = 0;


        _timer = 0;

        _cardController.Collider.enabled = true;

        DeactivateCheck(_distanceGO);
        DeactivateCheck(_checkBoundaryGO);
    }
}
