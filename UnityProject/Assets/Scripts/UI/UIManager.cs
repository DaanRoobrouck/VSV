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
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip[] _sounds;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _explanationText = _explanationPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _player = FindObjectOfType<FirstPersonAIO>();
        _pointsText.enabled = false;
        _explanationPanel.SetActive(false);
    }

    public void UpdateDestinationText(Transform transform)
    {
        _destinationText.text = string.Format("GA NAAR DE {0}", transform.name.ToString());
        _audio.clip = _sounds[1];
        _audio.Play();
    }

    public void UpdateExplanationText(String text)
    {
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        _explanationText.text = text.ToUpper();
       // _player.lockAndHideCursor = false;
        _player.playerCanMove = false;
        _player.enableCameraMovement = false;
        _audio.clip = _sounds[0];
        _audio.Play();
        _explanationPanel.SetActive(true);
    }

    public void DisableExplanation()
    {
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
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
