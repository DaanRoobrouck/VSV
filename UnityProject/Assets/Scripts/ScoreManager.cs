using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static float _score = 0;
    private int _concentrationScore = 0;
    private int _currentStreak = 1;

    //The concentrationScore and currentStreak values can be modified in the SituationController
    public int ConcentrationScore { get => _concentrationScore; set => _concentrationScore = value; }
    public int CurrentStreak { get => _currentStreak; set => _currentStreak = value; }

    [SerializeField] private int _mistake = 1;
    [SerializeField] private int _correct = 1;

    [SerializeField] private UIManager _uiManager;

    private void Start()
    {
        _score = 0;
    }
    public void AddScore(int time)
    {
        //float add = Mathf.Pow((_correct * (1 / time)), _currentStreak);
        //_score += add;
        _score += time;
        Debug.Log(time + " punten toegevoegd, totale score is: " + _score);
        _uiManager.UIPoints(time, true);
    }

    public void SubtractScore(int tries)
    {
        int subtract = _mistake * tries;
        _score -= subtract;
        Debug.Log(subtract + " punten verwijdert, totale score is: " + _score);
        _uiManager.UIPoints(subtract, false);
    }

    public void SendScore(string name, string email)
    {
        //Some logic that takes the name (string) of the player and the email (string) of the teacher
    }

    public void CalculateFinalScore()
    {
        _score = _score * _concentrationScore;
    }

    public void ResetScore()
    {
        _score = 0;
        _concentrationScore = 0;
        _currentStreak = 1;
    }
}
