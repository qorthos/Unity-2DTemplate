using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialoguePanel : MonoBehaviour
{
    public TMPro.TMP_Text Text;
    public TMPro.TMP_Text NameText;

    public Animator Animator;

    public UnityEvent OnShowFinished;
    public UnityEvent OnHideFinished;

    public void Show()
    {
        Animator.SetTrigger("Show");
    }

    public void Hide()
    {
        Animator.SetTrigger("Hide");
    }

    public void ShowComplete()
    {
        OnShowFinished?.Invoke();
    }

    public void HideComplete()
    {
        OnHideFinished?.Invoke();
        Text.text = "";
        NameText.text = "";
    }

}

