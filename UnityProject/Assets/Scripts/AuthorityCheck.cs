using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorityCheck : MonoBehaviour
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
            Debug.Log("Mag niet, je deed iets fout");
            SituationController.Tries++;
            _scoreManager.SubtractScore(SituationController.Tries);
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
