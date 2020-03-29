using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Situation { A, B, C, D, E}
public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _cardHolder;
    private FirstPersonAIO _player;

    [SerializeField] private List<int> _cardIndexOrder;
    private List<GameObject> _cards;
    public List<Card> PlayerOrder;

    public Situation AssignedSituation;

    private SituationController _situationController;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _panelCardHolder;
    private void Start()
    {
        _player = FindObjectOfType<FirstPersonAIO>();
        _situationController = GetComponent<SituationController>();

        _cards = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("In");
            //animatie, kaarten sliden in
            LeanTween.moveY(_cardHolder, 125, 0.5f);
            
            _player.lockAndHideCursor = false;
            _player.enableCameraMovement = false;
            Cursor.visible = true;

            if (_cards != null)
            {
                foreach (GameObject cardObject in _cards)
                {
                    _cards.Remove(cardObject);
                    Destroy(cardObject.gameObject);
                }
            }


            for (int i = 0; i < _cardIndexOrder.Count; i++)
            {
                GameObject cardObject = Instantiate(_cardPrefab, _panelCardHolder.transform);
                Card card = cardObject.GetComponent<Card>();
                card.Controller = this;

                _cards.Add(cardObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Out");
            //animatie, kaarten sliden uit
            LeanTween.moveY(_cardHolder, -125, 0.5f);
            
            _player.enableCameraMovement = true;
            _player.lockAndHideCursor = true;

            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckOrder();
        }
    }

    public void CheckOrder()
    {
        for (int i = 0; i < PlayerOrder.Count; i++)
        {
            if (PlayerOrder[i].Index == _cardIndexOrder[i])
            {
                Debug.Log("Juiste selectie");
                PlayerOrder[i].Btn.image.color = Color.green;

                //voorlopig
                _situationController.CorrectSequence = true;

                //foreach (GameObject cardObject in _cards)
                //{
                    //_cards.Remove(cardObject);
                    //Destroy(cardObject.gameObject);
                //}
            }
            else
            {
                Debug.Log("Onjuiste selectie");
                PlayerOrder[i].Btn.image.color = Color.red;
            }
        }
    }
}
