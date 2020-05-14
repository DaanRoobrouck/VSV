using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreLinkScript : MonoBehaviour
{
    [SerializeField]
    private Text _scoreTextField;

    // Start is called before the first frame update
    void Start()
    {
        _scoreTextField.text = ScoreManager._score.ToString();

        ScoreManager.ResetScore();
    }
}
