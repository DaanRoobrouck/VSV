using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPhase : MonoBehaviour
{
    [SerializeField] private GameObject _tutTxt;

    [SerializeField] private GameObject _DestinationBoard;
    // Start is called before the first frame 

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            StartCoroutine(PlayBuffer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (_tutTxt != null)
            {
                _tutTxt.SetActive(true);
                if (_DestinationBoard != null)
                {
                    _DestinationBoard.SetActive(true);
                }
            }
        }
    }

    private IEnumerator PlayBuffer()
    {
        yield return new WaitForSeconds(5f);
        if (_tutTxt != null)
        {
            _tutTxt.SetActive(true);
        }
    }
}
