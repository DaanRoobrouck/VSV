using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject levelSelectMenu;

    public Animator startButtonAnim;

    public void OpenSVSLink()
    {
        Application.OpenURL("https://www.verkeeropschool.be/");
    }
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void CloseLevelSelect()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OpenLevelSelect()
    {
        levelSelectMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Play()
    {
        Debug.Log("Starting game");
        startButtonAnim.SetTrigger("StartPressed");
    }
}
