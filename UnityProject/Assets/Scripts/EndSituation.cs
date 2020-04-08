using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSituation : MonoBehaviour
{
    private SituationController _situationController;
    private CardController _cardController;

    private void Start()
    {
        _situationController = this.transform.GetComponentInParent<SituationController>();
        _cardController = this.transform.GetComponentInParent<CardController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Situatie gereset");
            _situationController.ResetSituation();
            //_cardController.PlayerOrder.Clear();
        }
    }
}
