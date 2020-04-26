﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardCreator ScriptableCard;
    private bool _selected = false;

    private Image _icon;
    private Text _description;
    public Button Btn;

    private int _index;
    public int Index { get => _index;}

    public CardController Controller;

    private int _order;
    public int Order { get => _order; set => _order = value; }
    private Text _orderText;

    private void Start()
    {
        _icon = transform.GetChild(1).GetComponent<Image>();
        _description = GetComponentInChildren<Text>();
        
        _index = ScriptableCard.Index;
        Btn = GetComponent<Button>();
        _orderText = transform.GetChild(2).GetComponent<Text>();
        _orderText.text = string.Empty;

        _icon.sprite = ScriptableCard.Icon;
        _description.text = ScriptableCard.Description;
    }
    public void OnMouseEnter()
    {
        if (!_selected)
        {
            LeanTween.moveY(this.gameObject, 200, 0.25f);
        }
    }

    public void OnMouseExit()
    {
        if (!_selected)
        {
            LeanTween.moveY(this.gameObject, 125, 0.5f);
        }
    }

    public void OnMouseClick()
    {
        if (!_selected)
        {
            _selected = true;
            LeanTween.moveY(this.gameObject, 200, 0.25f);
            Controller.PlayerOrder.Add(this);
        }
        else
        {
            _selected = false;
            LeanTween.moveY(this.gameObject, 125, 0.5f);
            Controller.PlayerOrder.Remove(this);
            _orderText.text = string.Empty;
        }

        int index = 1;
        foreach (Card card in Controller.PlayerOrder)
        {
            card._orderText.text = index.ToString();
            index++;
        }
    }
}
