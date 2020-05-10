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
        _playerScript.lockAndHideCursor = false;
        Debug.Log("Zichtbaar");
        _playerScript.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _playerScript.enabled = false;
    }

    public void ContinueTutorial()
    {
        _animator.SetBool("ShowText", false);
        _playerScript.enabled = true;
        _playerScript.lockAndHideCursor = true;
        Debug.Log("Onzichtbaar");
        Destroy(this.transform.parent.gameObject);
        
    }

 
}
