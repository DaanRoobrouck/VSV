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
            Debug.Log("Mag niet, je deed iets fout");
            SituationController.Tries++;
            _scoreManager.SubtractScore(SituationController.Tries);
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
