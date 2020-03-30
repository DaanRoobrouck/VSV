using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float _score = 0;
    private int _concentrationScore = 0;
    private int _currentStreak = 1;

    //The concentrationScore and currentStreak values can be modified in the SituationController
    public int ConcentrationScore { get => _concentrationScore; set => _concentrationScore = value; }
    public int CurrentStreak { get => _currentStreak; set => _currentStreak = value; }

    [SerializeField] private int _mistake = 1;
    [SerializeField] private int _correct = 1;

    public void AddScore(float time)
    {
        _score += Mathf.Pow((_correct * (1 / time)), _currentStreak);
    }

    public void SubtractScore(int tries)
    {
        _score -= (_mistake * tries);
    }

    public void SendScore(string name, string email)
    {
        //Some logic that takes the name (string) of the player and the email (string) of the teacher
    }

    public void CalculateFinalScore()
    {
        _score = _score * _concentrationScore;
    }
}
