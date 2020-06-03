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

    private int _hints = 0;
    public int Hints { get => _hints; set => _hints = value; }

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
    [SerializeField] private GameObject _checkBoundary2GO;
    [SerializeField] private GameObject _distanceGO;
    [SerializeField] private BoxCollider _streetCollider;
    private Distance _distance;
    private AuthorityCheck _authorityCheck;
    //private GameObject _endGO;

    private ScoreManager _scoreManager;
    private float _timer;
    private float _stopTimer;

    private UIManager _uiManager;
    [SerializeField] private ParticleSystem _particle;

    private AudioSource _audio;
    [SerializeField] private AudioClip[] _sounds;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _authorityCheck = _checkBoundaryGO.GetComponent<AuthorityCheck>();
        _cardController = this.GetComponent<CardController>();
        _fpsController = FindObjectOfType<FirstPersonAIO>();
        _uiManager = FindObjectOfType<UIManager>();
        //_endGO = transform.GetChild(1).gameObject;
        _situation = _cardController.AssignedSituation;
        _scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));

        if (_checkBoundaryGO != null)
        {
            _authorityCheck.SituationController = this;
            //DeactivateCheck(_checkBoundaryGO);
        }
        if (_distanceGO != null)
        {
            _distance = _distanceGO.GetComponent<Distance>();
            _distance.SituationController = this;
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
            if (_timer >= 7)
            {
                Card card = _cardController.HintCardGO.GetComponent<Card>();
                _hints++;
                card.Description.enabled = true;
            }
            else
            {
                _timer += Time.deltaTime;
            }            
            switch (_situation)
            {
                case Situation.PedestrianCrossing:
                    if (!_hasStopped)
                    {
                        if (_checkBoundary2GO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundary2GO);
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
                        return;
                    }
                    break;
                case Situation.Obstacle:
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
                        if (_checkBoundaryGO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundaryGO);
                        }
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        return;
                    }
                    break;
                case Situation.BetweenCars:
                    if (!_hasStopped)
                    {
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
                        if (_streetCollider != null)
                        {
                            _streetCollider.enabled = false;
                        }
                        if (!_distanceGO.activeSelf)
                        {
                            ActivateCheck(_distanceGO);
                        }
                        return;
                    }
                    break;
                case Situation.Crossing:
                    if (!_hasStopped)
                    {
                        if (_checkBoundary2GO.activeSelf)
                        {
                            DeactivateCheck(_checkBoundary2GO);
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
        _uiManager.UpdateExplanationText("Je bent vergeten te stoppen aan de stoeprand!");

        if (_fpsController.fps_Rigidbody.velocity == Vector3.zero)
        {
            _stopTimer += Time.deltaTime;
            if (_stopTimer >= _stopTime)
            {
                Debug.Log("Time up, player stopped correct");
                _hasStopped = true;
                _stopTimer = 0;
                _scoreManager.AddScore(10, _uiManager);
                _cardController.HintIndex++;
                _cardController.HintCard(_cardController.HintIndex);
                _timer = 0;

                _particle.transform.position = _fpsController.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
            }
        }
    }

    private void CheckLookLeftRight()
    {
        _uiManager.UpdateExplanationText("Je bent vergeten te kijken naar links en rechts!");
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
                _particle.transform.position = _leftLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
                Debug.Log("Looked Left, now look right");
            }
            else if (_lookRight && hit.transform.gameObject == _rightLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookLeft = true;
                _particle.transform.position = _rightLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
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
            _scoreManager.AddScore(10, _uiManager);
            _cardController.HintIndex++;
            _cardController.HintCard(_cardController.HintIndex);
            _timer = 0;

            _particle.transform.position = _fpsController.transform.position;
            _particle.Play();
            _audio.clip = _sounds[0];
            _audio.Play();
        }
    }

    private void CheckLookCars()
    {
        _uiManager.UpdateExplanationText("Je bent vergeten te kijken naar de auto's voor je!");
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
                _particle.transform.position = _leftCarLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
            }
            else if (_lookRight && hit.transform.gameObject == _rightCarLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookLeft = true;
                Debug.Log("Looked at the right car");
                _particle.transform.position = _rightCarLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
            }
        }
        if (_leftCount >= 1 && _rightCount >= 1)
        {
            Debug.Log("Player looked correct");
            _hasLookedCars = true;
            _leftCount = 0;
            _rightCount = 0;
            _scoreManager.AddScore(10, _uiManager);
            _cardController.HintIndex++;
            _cardController.HintCard(_cardController.HintIndex);
            _timer = 0;

            _particle.transform.position = _fpsController.transform.position;
            _particle.Play();
            _audio.clip = _sounds[0];
            _audio.Play();
        }
    }

    private void CheckLookFourWay()
    {
        _uiManager.UpdateExplanationText("Je bent vergeten te kijken voor, achter, links en rechts van je!");
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
                _particle.transform.position = _leftLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
                Debug.Log("Looked Left, now look right");
            }
            else if (_lookRight && hit.transform.gameObject == _rightLookGO)
            {
                _rightCount++;
                _lookRight = false;
                _lookForward = true;
                _particle.transform.position = _rightLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
                Debug.Log("Looked Right, now look forward");
            }
            else if (_lookForward && hit.transform.gameObject == _forwardLookGO)
            {
                _forwardCount++;
                _lookForward = false;
                _lookBackward = true;
                _particle.transform.position = _forwardLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
                Debug.Log("Looked Forward, now look backward");
            }
            else if (_lookBackward && hit.transform.gameObject == _backwardLookGO)
            {
                _backwardCount++;
                _lookBackward = false;
                _lookLeft = true;
                _particle.transform.position = _backwardLookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
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

            _scoreManager.AddScore(10, _uiManager);
            _cardController.HintIndex++;
            _cardController.HintCard(_cardController.HintIndex);
            _timer = 0;

            _particle.transform.position = _fpsController.transform.position;
            _particle.Play();
            _audio.clip = _sounds[0];
            _audio.Play();
        }
    }

    private void CheckLookOneWay()
    {
        _uiManager.UpdateExplanationText("Je bent vergeten te kijken achter je!");
        ActivateCheck(_lookGO);

        RaycastHit hit;
        if (Physics.Raycast(_fpsController.transform.position, _fpsController.transform.forward, out hit, 100, LayerMask))
        {
            Debug.DrawRay(_fpsController.transform.position, _fpsController.transform.forward * 100, Color.red);
            if (hit.transform.gameObject == _lookGO)
            {
                Debug.Log("Player looked correct");
                DeactivateCheck(_lookGO);
                _scoreManager.AddScore(10, _uiManager);
                _hasLooked = true;
                _cardController.HintIndex++;
                _cardController.HintCard(_cardController.HintIndex);
                _timer = 0;

                _particle.transform.position = _lookGO.transform.position;
                _particle.Play();
                _audio.clip = _sounds[0];
                _audio.Play();
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
        _uiManager.UpdateExplanationText("Wat je nu doet is niet veilig! Je hebt de voorzorgsmaatregelen nog niet toegepast of moet ergens anders oversteken!");

        CorrectSequence = false;

        _scoreManager.AddScore(10, _uiManager);
        _audio.clip = _sounds[1];
        _audio.Play();

        _hasStopped = false;
        _hasLooked = false;
        _hasLookedCars = false;
        _checkDistance = false;

        _lookLeft = true;
        _lookRight = false;
        _leftCount = 0;
        _rightCount = 0;
        _forwardCount = 0;
        _backwardCount = 0;

        _timer = 0;
        _hints = 0;
        _tries = 0;

        foreach (GameObject indicationGO in _cardController.Indications)
        {
            indicationGO.SetActive(true);
        }

        if (_streetCollider != null)
        {
            _streetCollider.enabled = true;
        }

        _cardController.HintIndex = 0;
        Destroy(_cardController.HintCardGO);

        DeactivateCheck(_distanceGO);
        ActivateCheck(_checkBoundaryGO);
        if (_checkBoundary2GO != null)
        {
            ActivateCheck(_checkBoundary2GO);
        }     
    }

    public void EnableStartColliders()
    {
        _cardController.StartCollider1.enabled = true;
        if (_cardController.StartCollider2 != null)
        {
            _cardController.StartCollider2.enabled = true;
        }
        
    }
}
