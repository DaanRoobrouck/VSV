using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Situation { PedestrianCrossing, Obstacle, BetweenCars, Crossing}
public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _cardHolder;
    private FirstPersonAIO _player;

    [SerializeField] private List<int> _cardIndexOrder;
    [SerializeField] private List<CardCreator> _scriptableCards;
    private List<GameObject> _cards;
    public List<Card> PlayerOrder;

    public Situation AssignedSituation;

    private SituationController _situationController;

    [SerializeField] private GameObject _cardPrefab;

    public BoxCollider StartCollider1;
    public BoxCollider StartCollider2;

    [SerializeField] private GameObject _endCollider;

    private bool _hidecards = false;
    private bool _isActive = false;

    [SerializeField] private bool _bothWays = false;
    private void Start()
    {
        _player = FindObjectOfType<FirstPersonAIO>();
        _situationController = GetComponent<SituationController>();

        _cards = new List<GameObject>();

        _endCollider.SetActive(false);

        if (!_bothWays)
        {
            StartCollider1 = this.GetComponent<BoxCollider>();
        }       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowCards();

            Debug.Log("In");
            //animatie, kaarten sliden in
            LeanTween.moveY(_cardHolder, 125, 0.5f);
            
            _player.lockAndHideCursor = false;
            _player.enableCameraMovement = false;
            Cursor.visible = true;

            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideCards();

            //animatie, kaarten sliden uit
            LeanTween.moveY(_cardHolder, -125, 0.5f);

            PlayerOrder.Clear();

            FreezePlayer(false);         
        }
    }

    public void FreezePlayer(bool state)
    {
        if (!state)
        {
            _player.enableCameraMovement = true;
            _player.lockAndHideCursor = true;
            Cursor.visible = false;
        }
        else
        {            
            _player.lockAndHideCursor = false;
            _player.enableCameraMovement = false;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Check");
                CheckOrder();
            }

            if (_hidecards)
            {
                foreach (GameObject cardObject in _cards)
                {
                    Destroy(cardObject.gameObject);
                }
                _cards.Clear();
                if (_cards.Count == 0)
                {
                    _hidecards = false;
                    _isActive = false;
                }
            }
        }
    }

    private void ShowCards()
    {
        for (int i = 0; i < _cardIndexOrder.Count; i++)
        {
            GameObject cardObject = Instantiate(_cardPrefab, _cardHolder.transform);
            Card card = cardObject.GetComponent<Card>();
            card.ScriptableCard = _scriptableCards[i];
            card.Controller = this;

            _cards.Add(cardObject);
        }
    }

    private void HideCards()
    {
        _hidecards = true;
    }

    public void CheckOrder()
    {
        if (PlayerOrder.Count != _cardIndexOrder.Count)
        {
            Debug.Log("Select all cards pls");
            return;
        }
        int incorrect = 0;
        for (int i = 0; i < PlayerOrder.Count; i++)
        {
            if (PlayerOrder[i].Index == _cardIndexOrder[i])
            {
                Debug.Log("Juiste selectie");
                PlayerOrder[i].Btn.image.color = Color.green;
            }
            else
            {
                Debug.Log("Onjuiste selectie");
                PlayerOrder[i].Btn.image.color = Color.red;
                incorrect++;
            }
        }
        if (incorrect == 0)
        {
            HideCards();
            _situationController.CorrectSequence = true;
            if (_bothWays)
            {
                StartCollider1.enabled = false;
                StartCollider2.enabled = false;
            }
            else
            {
                StartCollider1.enabled = false;
            }
            

            _endCollider.SetActive(true);

            PlayerOrder.Clear();

            FreezePlayer(false);
        }
    }
}
