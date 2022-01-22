using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleSceneController : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public GameDataChannel GameDataChannel;
    public GameObject AllScreensContainer;


    void Start()
    {

    }


    void Update()
    {

    }

    public void Button_StartNewGameScreen()
    {
        var rectTransform = AllScreensContainer.GetComponent<RectTransform>();
        rectTransform.DOLocalMove(new Vector3(-1480, 0, 0), 0.75f, false).SetEase(Ease.InOutBack);
    }

    public void Button_ContinueGameScreen()
    {
        var rectTransform = AllScreensContainer.GetComponent<RectTransform>();
        rectTransform.DOLocalMove(new Vector3(+1480, 0, 0), 0.75f, false).SetEase(Ease.InOutBack);
    }

    public void Button_OptionsScreen()
    {
        var rectTransform = AllScreensContainer.GetComponent<RectTransform>();
        rectTransform.DOLocalMove(new Vector3(0, -920, 0), 0.75f, false).SetEase(Ease.InOutBack);
    }

    public void Button_TestsScreen()
    {
        var rectTransform = AllScreensContainer.GetComponent<RectTransform>();
        rectTransform.DOLocalMove(new Vector3(0, +920, 0), 0.75f, false).SetEase(Ease.InOutBack);
    }

    public void Button_MainScreen()
    {
        var rectTransform = AllScreensContainer.GetComponent<RectTransform>();
        rectTransform.DOLocalMove(new Vector3(0, 0, 0), 0.75f, false).SetEase(Ease.InOutBack);
    }

    public void Button_ExitGame()
    {
        Application.Quit();
    }

    public void Button_StartNewGame(int slot)
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Scissor,
            NewSceneName = "ManagementScene",
            OnClose = new System.Action(()=>
            {
                GameDataChannel.StartNewGame(slot);
            }),
        });
    }

    public void Button_ContinueGame(int slot)
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Scissor,
            NewSceneName = "ManagementScene",
            OnClose = new System.Action(() =>
            {
                GameDataChannel.LoadFromPrefs(slot);
            }),
        });
    }

    public void Button_ClearSaveData()
    {
        GameDataChannel.ClearPlayerPrefs();
    }

    public void Button_TestFadeTransition()
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Fullscreen,
            TransitionInEffect = TransitionEffectEnum.Fullscreen,
            NewSceneName = "TitleScene",
        });
    }

    public void Button_TestScissorTransition()
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Scissor,
            TransitionInEffect = TransitionEffectEnum.Scissor,
            NewSceneName = "TitleScene",
        });
    }

    public void Button_TestWheelTransition()
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Wheel,
            TransitionInEffect = TransitionEffectEnum.Wheel,
            NewSceneName = "TitleScene",
        });
    }

    public void Button_TestNarrative()
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Scissor,
            NewSceneName = "NarrativeScene",
            OnClose = new System.Action(() =>
            {

            }),
        });
    }

    public void Button_TestDialogue()
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            TransitionOutEffect = TransitionEffectEnum.Scissor,
            NewSceneName = "DialogueScene",
            OnClose = new System.Action(() =>
            {

            }),
        });
    }

    public void Button_TestModalOK()
    {
        GameEventChannel.Broadcast(new ShowModalOKGEM()
        {
            PanelMessage = "This concludes the test.",
            ButtonMessage = "Confirm",
            OKAction = new System.Action(()=>Debug.Log("Show Modal OK Action")),            
        });
    }
}
