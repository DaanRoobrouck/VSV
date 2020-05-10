using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorityCheck : MonoBehaviour
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
            if (SituationController != null)
            {
                SituationController.Tries++;
                _scoreManager.SubtractScore(SituationController.Tries, _uiManager);
            }
            else
            {
                _scoreManager.SubtractScore(1, _uiManager);
            }
            _uiManager.UpdateExplanationText("Wat je nu doet is niet veilig! Je hebt de voorzorgsmaatregelen nog niet toegepast of moet ergens anders oversteken!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Terug op het goede pad");
        }
    }
}
