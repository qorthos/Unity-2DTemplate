using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showable : MonoBehaviour, IShowable
{
    [ReadOnly] [SerializeField] bool isShown = false;
    public Animator Animator;
    public bool IsShown { get => isShown; private set => isShown = value; }
    

    public virtual void Hide()
    {
        Animator.SetTrigger("Hide");
        IsShown = false;
    }

    public virtual void Show()
    {
        Animator.SetTrigger("Show");
        IsShown = true;
    }


}

