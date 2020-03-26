using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    public void OpenSVSLink()
    {
        Application.OpenURL("https://www.verkeeropschool.be/");
    }
    public void OpenSettings()
    {
        Debug.Log("Opening settings");
    }
    public void Play()
    {
        Debug.Log("Starting game");
    }
}
