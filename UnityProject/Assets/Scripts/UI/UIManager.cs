using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _destinationText;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private GameObject _explanationPanel;
    private TextMeshProUGUI _explanationText;
    private FirstPersonAIO _player;

    void Start()
    {
        _explanationText = _explanationPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _player = FindObjectOfType<FirstPersonAIO>();
        _pointsText.enabled = false;
        DisableExplanation();
    }

    public void UpdateDestinationText(Transform transform)
    {
        _destinationText.text = string.Format("GA NAAR {0}", transform.name.ToString());
    }

    public void UpdateExplanationText(String text)
    {
        _explanationText.text = text.ToUpper();
       // _player.lockAndHideCursor = false;
        _player.playerCanMove = false;
        _player.enableCameraMovement = false;

        _explanationPanel.SetActive(true);
    }

    public void DisableExplanation()
    {
        _explanationPanel.SetActive(false);
        _player.enableCameraMovement = true;
        _player.playerCanMove = true;
        //_player.lockAndHideCursor = true;
    }

    public void UIPoints(int value, bool status)
    {
        string sign;
        if (status)
        {
            _pointsText.color = Color.green;
            sign = "+";
        }
        else
        {
            _pointsText.color = Color.red;
            sign = "-";
        }
        _pointsText.enabled = true;
        _pointsText.text = sign + value.ToString().ToUpper();
        StartCoroutine(FadeTextIn(2));
        
    }

    private IEnumerator FadeTextIn(int time)
    {
        yield return new WaitForSeconds(time);
        _pointsText.CrossFadeAlpha(0, time, false);
        StartCoroutine(FadeTextOut(2));
    }

    private IEnumerator FadeTextOut(int time)
    {
        yield return new WaitForSeconds(time);
        _pointsText.CrossFadeAlpha(1, time, false);
        _pointsText.enabled = false;
    }
}
