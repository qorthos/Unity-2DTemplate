using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionSystem : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public Animator Animator;

    [ReadOnly] [SerializeField] TransitionGEM transition;
    //public string TargetSceneName;
    //public TransitionEffectEnum TransitionOut;
    //public TransitionEffectEnum TransitionIn;
    //public Action OnClose;
    //public Action OnReopen;

    private void Awake()
    {
        GameEventChannel.RegisterListener<TransitionGEM>(OnTransition);   
    }

    private void OnTransition(TransitionGEM arg0)
    {
        transition = arg0;
        

        switch (arg0.TransitionOutEffect)
        {
            case TransitionEffectEnum.Fullscreen:
                Animator.SetTrigger("FullscreenFadeOut");                
                break;

            case TransitionEffectEnum.Scissor:
                Animator.SetTrigger("ScissorFadeOut");
                break;

            case TransitionEffectEnum.Wheel:
                Animator.SetTrigger("WheelFadeOut");
                break;

            default:
                Debug.LogError($"TransitionSystem does not implement: {arg0.TransitionOutEffect}");
                Animator.SetTrigger("FullscreenFadeOut");
                break;
        }

        Animator.SetBool("SnapFadeIn", false);
        Animator.SetBool("FullscreenFadeIn", false);
        Animator.SetBool("ScissorFadeIn", false);
        Animator.SetBool("WheelFadeIn", false);

        switch (arg0.TransitionInEffect)
        {
            case TransitionEffectEnum.Fullscreen:
                Animator.SetBool("FullscreenFadeIn", true);
                break;

            case TransitionEffectEnum.Scissor:
                Animator.SetBool("ScissorFadeIn", true);
                break;

            case TransitionEffectEnum.Wheel:
                Animator.SetBool("WheelFadeIn", true);
                break;

            case TransitionEffectEnum.None:
                Animator.SetBool("SnapFadeIn", true);
                break;

            default:
                Debug.LogError($"TransitionSystem does not implement: {arg0.TransitionInEffect}");
                Animator.SetBool("SnapFadeIn", true);
                break;
        }
    }

    public void TransitionCloseComplete()
    {
        transition.OnClose?.Invoke();

        if (transition.NewSceneName != "")
        {
            SceneManager.LoadScene(transition.NewSceneName);
        }
    }

    public void TransitionReOpenComplete()
    {
        transition.OnReopen?.Invoke();
    }
}

public enum TransitionEffectEnum
{
    None,
    Fullscreen,
    Scissor,
    Wheel,
}


public class TransitionGEM : EventArgs
{
    public string NewSceneName = "";
    public TransitionEffectEnum TransitionOutEffect;
    public TransitionEffectEnum TransitionInEffect = TransitionEffectEnum.None;
    public Action OnClose;
    public Action OnReopen;
}