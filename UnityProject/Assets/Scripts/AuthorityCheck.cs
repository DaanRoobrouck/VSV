using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorityCheck : MonoBehaviour
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
            Debug.Log("Mag niet, je deed iets fout");
            _situationController.Tries++;
            _scoreManager.SubtractScore(_situationController.Tries);
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
