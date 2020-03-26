using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    public float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 4)
        {
            Destroy(this.gameObject);
        }
    }
}
