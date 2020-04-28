using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationBehaviour : MonoBehaviour
{
    private bool _moveTextDown = false;
    private Vector3 _destination;

    void Start()
    {
        _destination = this.transform.position;
    }

    void Update()
    {
        if (this.enabled == true)
        {
            if (!_moveTextDown)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, _destination + Vector3.up * 0.5f, 1f * Time.deltaTime);

                if (Vector3.Distance(this.transform.position, _destination + Vector3.up * 0.5f) < 0.1f)
                {
                    _moveTextDown = true;
                }
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, _destination, 1f * Time.deltaTime);

                if (Vector3.Distance(this.transform.position, _destination) < 0.1f)
                {
                    _moveTextDown = false;
                }
            }
        }
    }
}
