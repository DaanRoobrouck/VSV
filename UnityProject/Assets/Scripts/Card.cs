using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool _selected = false;

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
        }
        else
        {
            _selected = false;
            LeanTween.moveY(this.gameObject, 125, 0.5f);
        }
    }
}
