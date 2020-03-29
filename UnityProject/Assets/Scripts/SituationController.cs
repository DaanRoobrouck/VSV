using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituationController : MonoBehaviour
{
    private CardController _cardController;
    private FirstPersonAIO _fpsController;
    private Situation _situation;

    private bool _hasStopped = false;
    private bool _hasLooked = false;
    private bool _checkDistance = false;

    private bool _lookLeft = true;
    private bool _lookRight = false;
    private int _leftCount = 0;
    private int _rightCount = 0;

    public bool CorrectSequence = false;

    private float _timer;
    [SerializeField] private float _stopTime;
    [SerializeField] private int _aantalKeerKijken = 2;

    public LayerMask LayerMask;

    [SerializeField] private GameObject _lookGO;
    [SerializeField] private GameObject _leftLookGO;
    [SerializeField] private GameObject _rightLookGO;
    private GameObject _distanceGO;
    private GameObject _endGO;

    void Start()
    {
        _cardController = this.GetComponent<CardController>();
        Debug.Log("name: " + _cardController.gameObject.name);
        _fpsController = FindObjectOfType<FirstPersonAIO>();
        _distanceGO = transform.GetChild(0).gameObject;
        _endGO = transform.GetChild(1).gameObject;
        _situation = _cardController.AssignedSituation;

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
    }

    void Update()
    {
        if (CorrectSequence)
        {
            switch (_situation)
            {
                case Situation.A:
                    if (!_hasStopped)
                    {
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
                        ActivateCheck(_distanceGO);
                        return;
                    }
                    break;
                case Situation.B:
                    if (!_hasStopped)
                    {
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
                        ActivateCheck(_distanceGO);
                        return;
                    }
                    break;
                case Situation.C:
                    break;
                case Situation.D:
                    break;
                case Situation.E:
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
            _timer += Time.deltaTime;
            if (_timer >= _stopTime)
            {
                Debug.Log("Time up, player stopped correct");
                _hasStopped = true;
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
            //Kan beter gechecked worden
            if (_lookLeft && hit.transform.name == "LeftLookCollider")
            {
                _leftCount++;
                _lookLeft = false;
                _lookRight = true;
                Debug.Log("Looked Left, now look right");
            }
            else if (_lookRight && hit.transform.name == "RightLookCollider")
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
        }
    }

    private void CheckLookOneWay()
    {
        ActivateCheck(_lookGO);

        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (hit.transform.name == "LookCollider")
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
        _hasStopped = false;
        _hasLooked = false;
        _checkDistance = false;

        _lookLeft = true;
        _lookRight = false;
        _leftCount = 0;
        _rightCount = 0;

        CorrectSequence = false;

        _timer = 0;

        DeactivateCheck(_distanceGO);
    }
}
