﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject levelSelectMenu;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject transitionScreen;
    [SerializeField]
    private FirstPersonAIO playerController;

    private Animator _anim;

    private void Start()
    {
        _anim = this.GetComponent<Animator>();
    }

    public void OpenSVSLink()
    {
        Application.OpenURL("https://www.vsv.be/");
    }
    public void OpenSettingsInMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void CloseSettingsInMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OpenSettingsInGame()
    {
        settingsMenu.SetActive(true);
    }
    public void CloseSettingsInGame()
    {
        settingsMenu.SetActive(false);
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
        _anim.SetTrigger("StartPressed");
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void LoadCity()
    {
        StartCoroutine(LoadingProcess());
    }

    IEnumerator LoadingProcess()
    {
        transitionScreen.GetComponent<Animator>().SetTrigger("LoadingStarted");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("CityMockup");
    }

    public void ClosePauseScreen()
    {
        pauseScreen.GetComponent<Animator>().SetTrigger("ContinuePressed");
        playerController.enableCameraMovement = true;
        playerController.playerCanMove = true;

    }
    public void OpenPauseScreen()
    {
        pauseScreen.GetComponent<Animator>().SetTrigger("PausePressed");
        playerController.enableCameraMovement = false;
        playerController.playerCanMove = false;
    }
}
