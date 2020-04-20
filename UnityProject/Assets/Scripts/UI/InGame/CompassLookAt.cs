using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassLookAt : MonoBehaviour
{
    public Transform Target;

    private Vector3 dir;
    private float angle;

    // Update is called once per frame
    void Update()
    {
        //dir = Target.transform.position - transform.position;
        //angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //this.transform.eulerAngles = new Vector3(0, 0, angle);
        this.transform.LookAt(Target.transform.position);
        this.transform.rotation.Set(0, 0, this.transform.rotation.z, 0);
    }
}
