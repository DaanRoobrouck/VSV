using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    public SituationController SituationController;
    private ScoreManager _scoreManager;
    private void Start()
    {
        _scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Je bent op de goede weg");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SituationController.Tries++;
            _scoreManager.SubtractScore(SituationController.Tries);
            Debug.Log("Blijf op het pad aub");
        }
    }
}
