using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextScene : MonoBehaviour
{

    public GameObject transitionScreen;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LoadingProcess("ScoreDisplay"));
    }

    IEnumerator LoadingProcess(string LevelName)
    {
        transitionScreen.GetComponent<Animator>().SetTrigger("LoadingStarted");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(LevelName);
    }
}
