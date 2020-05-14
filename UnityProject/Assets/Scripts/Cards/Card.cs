using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardCreator ScriptableCard;
    public bool Selected = false;

    public Image Icon;
    public Text Description;
    public Button Btn;

    private int _index;
    public int Index { get => _index;}

    public CardController Controller;

    private int _order;
    public int Order { get => _order; set => _order = value; }
    private Text _orderText;
    private AudioSource _audio;
    [SerializeField] private AudioClip _sound;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        Icon = transform.GetChild(1).GetComponent<Image>();
        Description = GetComponentInChildren<Text>();
        
        _index = ScriptableCard.Index;
        Btn = GetComponent<Button>();
        _orderText = transform.GetChild(2).GetComponent<Text>();
        _orderText.text = string.Empty;

        Icon.sprite = ScriptableCard.Icon;
        Description.text = ScriptableCard.Description;
    }
    public void OnMouseEnter()
    {
        if (!Selected)
        {
            LeanTween.moveY(this.gameObject, 200, 0.25f);
        }
    }

    public void OnMouseExit()
    {
        if (!Selected)
        {
            LeanTween.moveY(this.gameObject, 125, 0.5f);
        }
    }

    public void OnMouseClick()
    {
        if (!Selected)
        {
            Selected = true;
            LeanTween.moveY(this.gameObject, 200, 0.25f);
            Controller.PlayerOrder.Add(this);
            _audio.clip = _sound;
            _audio.Play();
        }
        else
        {
            Selected = false;
            LeanTween.moveY(this.gameObject, 125, 0.5f);
            Controller.PlayerOrder.Remove(this);
            _orderText.text = string.Empty;
            Btn.image.color = Color.white;
        }

        int index = 1;
        foreach (Card card in Controller.PlayerOrder)
        {
            card._orderText.text = index.ToString();
            index++;
        }
    }
}
