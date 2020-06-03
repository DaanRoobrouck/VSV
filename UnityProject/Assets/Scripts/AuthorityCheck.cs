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

                if (!SituationController.CorrectSequence)
                {
                    _uiManager.UpdateExplanationText("Je moet eerst de kaarten in de juiste volgorde selecteren voor je verder kan!");
                }
            }
            else if (transform.parent.CompareTag("Light"))
            {
                _uiManager.UpdateExplanationText("Het licht staat op rood, je mag nu niet oversteken! Wacht tot het licht op groen springt!");
            }
            else
            {           
                _scoreManager.SubtractScore(1, _uiManager);
                Debug.Log(this.gameObject.name);
            }
            _uiManager.ShowExplanationText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiManager.UpdateExplanationText("Wat je nu doet is niet veilig! Je hebt de voorzorgsmaatregelen nog niet toegepast of moet ergens anders oversteken!");
        }
    }
}
