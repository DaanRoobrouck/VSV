using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreLinkScript : MonoBehaviour
{
    [SerializeField]
    private Text _scoreTextField;
    [SerializeField]
    private Slider _selfGradeSlider;
    [SerializeField]
    private GameObject _emailConfirmationText;

    public static float SelfScore;
    // Start is called before the first frame update
    void Start()
    {
        _scoreTextField.text = ScoreManager._score.ToString();

    }
    private void Update()
    {
        SelfScore = _selfGradeSlider.value;
    }
    public void ResetScore()
    {
        ScoreManager.ResetScore();
    }
    public void ConfirmEmailSent()
    {
        _emailConfirmationText.SetActive(true);
    }
}
