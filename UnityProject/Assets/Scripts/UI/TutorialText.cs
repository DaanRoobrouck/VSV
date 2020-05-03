using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private FirstPersonAIO _playerScript;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _playerScript.enabled = false;
        _playerScript.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueTutorial()
    {
        _animator.SetBool("ShowText", false);
        _playerScript.enabled = true;
        Destroy(this.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

 
}
