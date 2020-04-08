using System.Collections;
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

    private void Start()
    {
        _icon = transform.GetChild(1).GetComponent<Image>();
        _description = GetComponentInChildren<Text>();
        
        _index = ScriptableCard.Index;
        Btn = GetComponent<Button>();

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
        }
    }
}
