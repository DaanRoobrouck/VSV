using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSituation : MonoBehaviour
{
    private SituationController _situationController;


    private void Start()
    {
        _situationController = this.transform.GetComponentInParent<SituationController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Situatie gereset");
            _situationController.ResetSituation();
        }
    }
}
