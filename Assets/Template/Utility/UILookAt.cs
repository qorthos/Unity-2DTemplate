using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UILookAt : MonoBehaviour
{
    public GameObject Target;

    // Update is called once per frame
    void Update()
    {
        if (Target == null)
            return;

        transform.LookAt(transform.position + Target.transform.forward);
    }
}
