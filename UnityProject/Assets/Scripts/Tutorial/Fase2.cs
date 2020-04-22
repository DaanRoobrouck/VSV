﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase2 : MonoBehaviour
{
    [SerializeField] private GameObject _tutTxt;
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