using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    [ReadOnly] [SerializeField] bool isOpen = false;
    public Animator Animator;
    public bool IsOpen { get => isOpen; private set => isOpen = value; }
    

    public void Close()
    {
        Animator.SetTrigger("Close");
        IsOpen = false;
    }

    public void Open()
    {
        Animator.SetTrigger("Open");
        IsOpen = true;
    }


}

