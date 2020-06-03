using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Situation { PedestrianCrossing, Obstacle, BetweenCars, Crossing}
public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _cardHolder;
    [SerializeField] private GameObject _checkButtonGO;
    private Button _checkButton;
    private FirstPersonAIO _player;

    private List<int> _cardIndexOrder = new List<int>();
    [SerializeField] public List<CardCreator> ScriptableCards;
    private List<GameObject> _cards;
    public List<Card> PlayerOrder;

    public Situation AssignedSituation;

    private SituationController _situationController;

    [SerializeField] private GameObject _cardPrefab;

    public BoxCollider StartCollider1;
    public BoxCollider StartCollider2;

    public List<GameObject> Indications = new List<GameObject>();

    [SerializeField] private GameObject _endCollider;
    [SerializeField] private GameObject _hintCard;

    private bool _hidecards = false;
    private bool _isActive = false;

    public GameObject HintCardGO;
    public int HintIndex = 0;

    [SerializeField] private bool _bothWays = false;
    private AudioSource _audio;
    [SerializeField] private AudioClip[] _sounds;
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        foreach (CardCreator card in ScriptableCards)
        {
            _cardIndexOrder.Add(card.Index);
        }
        _checkButtonGO.SetActive(false);
        _checkButton = _checkButtonGO.GetComponent<Button>();
        
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
            //_player.transform.LookAt(_endCollider.transform);

            foreach (GameObject indicationGO in Indications)
            {
                indicationGO.SetActive(false);
            }
            ShowCards();
            _checkButton.onClick.AddListener(CheckOrder);
            Debug.Log("In");
            //animatie, kaarten sliden in
            LeanTween.moveY(_cardHolder, 125, 0.5f);

            //_player.lockAndHideCursor = false;

            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;

            Debug.Log("Maximzichtbaar");
            _player.enableCameraMovement = false;
            //Cursor.visible = true;

            _isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject indicationGO in Indications)
            {
                indicationGO.SetActive(true);
            }
            HideCards();
            _checkButton.onClick.RemoveListener(CheckOrder);
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
            //_player.lockAndHideCursor = true;

            Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
            Debug.Log("MaximzOnichtbaar");

            //Cursor.visible = false;
        }
        else
        {
            //_player.lockAndHideCursor = false;

            _player.enableCameraMovement = false;
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
            Debug.Log("Maximzichtbaar");

            //Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            if (PlayerOrder.Count == _cardIndexOrder.Count)
            {
                _checkButtonGO.SetActive(true);
            }
            else
            {
                _checkButtonGO.SetActive(false);
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
        List<CardCreator> cards = new List<CardCreator>(ScriptableCards);
        while (cards.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, cards.Count - 1);

            GameObject cardObject = Instantiate(_cardPrefab, _cardHolder.transform);
            Card card = cardObject.GetComponent<Card>();
            card.ScriptableCard = ScriptableCards[ScriptableCards.IndexOf(cards[randomIndex])];
            card.Controller = this;

            _cards.Add(cardObject);
            cards.RemoveAt(randomIndex);
        }
        //for (int i = 0; i < _cardIndexOrder.Count; i++)
        //{
        //    GameObject cardObject = Instantiate(_cardPrefab, _cardHolder.transform);
        //    Card card = cardObject.GetComponent<Card>();
        //    card.ScriptableCard = ScriptableCards[i];
        //    card.Controller = this;

        //    _cards.Add(cardObject);
        //}
    }

    private void HideCards()
    {
        _hidecards = true;
    }

    public void CheckOrder()
    {
        int incorrect = 0;
        for (int i = 0; i < PlayerOrder.Count; i++)
        {
            if (PlayerOrder[i].Index == _cardIndexOrder[i])
            {
                Debug.Log("Juiste selectie");
                PlayerOrder[i].Btn.image.color = Color.green;
            }
            else if (i != 0 && PlayerOrder[i].Index == _cardIndexOrder[i - 1])
            {
                Debug.Log("Onjuiste selectie");
                PlayerOrder[i].Btn.image.color = Color.yellow;
                incorrect++;
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
            _audio.clip = _sounds[0];
            _audio.Play();
            _checkButton.onClick.RemoveAllListeners();
            _checkButtonGO.SetActive(false);
            HintCard(HintIndex);
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
        else 
        {
            _audio.clip = _sounds[1];
            _audio.Play();
        }
    }

    public void HintCard(int index)
    {
        if (index > ScriptableCards.Count)
        {
            Destroy(HintCardGO);
            return;
        }
        if (HintCardGO == null)
        {
            HintCardGO = Instantiate(_cardPrefab, _hintCard.transform);
        }
        Card card = HintCardGO.GetComponent<Card>();
        card.ScriptableCard = ScriptableCards[index];

        Image icon = card.transform.GetChild(1).GetComponent<Image>();
        Text description = card.GetComponentInChildren<Text>();
        icon.sprite = card.ScriptableCard.Icon;
        description.text = card.ScriptableCard.Description;
        description.enabled = false;

        card.Controller = this;
        card.Selected = true;

        Button btn = card.GetComponent<Button>();
        btn.interactable = false;
    }
}
