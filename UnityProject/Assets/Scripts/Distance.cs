using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    private SituationController _situationController;
    private ScoreManager _scoreManager;
    private void Start()
    {
        _situationController = this.GetComponentInParent<SituationController>();
        _scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _situationController.Tries++;
            _scoreManager.SubtractScore(_situationController.Tries);
            Debug.Log("Terug op goede pad");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Blijf op het pad aub");
        }
    }
}
