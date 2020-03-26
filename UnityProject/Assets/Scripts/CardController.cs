using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _cardHolder;
    private FirstPersonAIO _player;

    private void Start()
    {
        _player = FindObjectOfType<FirstPersonAIO>();
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
}
