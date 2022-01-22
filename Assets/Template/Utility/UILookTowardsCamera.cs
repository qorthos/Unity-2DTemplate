using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UILookTowardsCamera : MonoBehaviour
{
    GameObject camTarget;

    private void Start()
    {
        camTarget = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {        
        if (camTarget == null)
            return;

        transform.LookAt(transform.position + camTarget.transform.forward);
    }
}
