using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    public GameEventChannel GameEventChannel;
    public GameDataChannel GameDataChannel;
    public YarnProgramsChannel YarnProgramsChannel;
    public SpriteLibrary SpriteLibrary;
    public DialogueRunner DialogueRunner;
    public DialogueUI DialogueUI;

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    private void Awake()
    {
        GameEventChannel.RegisterListener<DialogueRequestedGEM>(OnDialogueRequested);
        GameEventChannel.RegisterListener<GameQuitGEM>(OnGameQuit);

        if (YarnProgramsChannel.DialoguePrograms.Count > 0)
        {
            DialogueRunner.Dialogue.SetProgram(YarnProgramsChannel.Program);
        }
        else
        {
            Debug.LogWarning("No dialogue files found in YarnProgramsChannel");
        }
        
        foreach (YarnProgram prog in YarnProgramsChannel.DialoguePrograms)
        {
            DialogueRunner.AddStringTable(prog);
        }

        DialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        DialogueRunner.AddCommandHandler("ChangeScene", ChangeScene);
        DialogueRunner.AddCommandHandler("OptionsCaption", OptionsCaption);
    }

    private void OnDestroy()
    {
        GameEventChannel.RemoveListener<DialogueRequestedGEM>(OnDialogueRequested);
        GameEventChannel.RemoveListener<GameQuitGEM>(OnGameQuit);
    }


    private void OptionsCaption(string[] parameters)
    {
        var sb = new StringBuilder();
        foreach (string part in parameters)
        {
            sb.Append(part);
            sb.Append(" ");
        }

        DialogueUI.DialogueOptions.SetCaption(sb.ToString());
    }

    private void OnDialogueComplete()
    {
        GameEventChannel.Broadcast(new DialogueFinishedGEM());
    }

    private void OnDialogueRequested(DialogueRequestedGEM arg0)
    {
        DialogueRunner.YarnScriptOnCompleteAction = arg0.OnComplete;
        DialogueRunner.StartDialogue(arg0.NodeName);
        GameEventChannel.Broadcast(new DialogueStartedGEM());
    }


    private void OnGameQuit(GameQuitGEM arg0)
    {
        DialogueRunner.Stop();
    }


    private void ChangeScene(string[] parameters)
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            NewSceneName = parameters[0],
            TransitionOutEffect = TransitionEffectEnum.Fullscreen,
        });
    }
}

public class DialogueRequestedGEM : YarnArgs
{
    public string NodeName;
}

public class DialogueStartedGEM : EventArgs
{

}

public class DialogueFinishedGEM : EventArgs
{

}