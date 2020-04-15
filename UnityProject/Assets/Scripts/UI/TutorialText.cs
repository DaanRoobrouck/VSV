using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private FirstPersonAIO _playerScript;
    [SerializeField] private Animator _animator;

    [SerializeField] private string _shownText;

    private void OnTriggerEnter(Collider other)
    {
        _playerScript.enabled = false;
    }

    public void ContinueTutorial()
    {
        _animator.SetBool("ShowText", false);
        _playerScript.enabled = true;
    }
}
