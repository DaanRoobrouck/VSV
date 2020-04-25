using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    public SituationController SituationController;
    private ScoreManager _scoreManager;
    private UIManager _uiManager;
    private void Start()
    {
        _scoreManager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));
        _uiManager = FindObjectOfType<UIManager>();
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
            _uiManager.UpdateExplanationText("Blijf op het voetpad, het is niet veilig om nu op straat te lopen!");
        }
    }
}
