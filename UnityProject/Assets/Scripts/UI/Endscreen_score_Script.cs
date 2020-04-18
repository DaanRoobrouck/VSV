using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Endscreen_score_Script : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GetComponent<TextMeshPro>().text = ScoreManager._score.ToString();
    }

}
