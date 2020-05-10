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
        //_playerScript.lockAndHideCursor = false;
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        Debug.Log("Zichtbaar");
        _playerScript.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _playerScript.enableCameraMovement = false;
        _playerScript.playerCanMove = false;
        
    }

    public void ContinueTutorial()
    {
        //_playerScript.lockAndHideCursor = true;

        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;

        _animator.SetBool("ShowText", false);
        _playerScript.enableCameraMovement = true;
        _playerScript.playerCanMove = true;
        Debug.Log("Onzichtbaar");
        Destroy(this.transform.parent.gameObject);
        
    }

 
}
