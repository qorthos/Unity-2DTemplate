using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn;

public class NarrativeSystem : MonoBehaviour
{
    public static NarrativeSystem Instance;

    public GameEventChannel GameEventChannel;
    public GameDataChannel GameDataChannel;
    public YarnProgramsChannel YarnProgramsChannel;
    public SpriteLibrary SpriteLibrary;
    public NarrativeRunner NarrativeRunner;
    public GameObject DialogueContainer;
    public GameObject SpritePanel;

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
        GameEventChannel.RegisterListener<NarrativeRequestedGEM>(OnNarrativeRequested);
        GameEventChannel.RegisterListener<GameQuitGEM>(OnGameQuit);
    }

    private void Start()
    {
        if (YarnProgramsChannel.DialoguePrograms.Count > 0)
        {
            NarrativeRunner.Dialogue.SetProgram(YarnProgramsChannel.Program);
        }
        else
        {
            Debug.LogWarning("No dialogue files found in YarnProgramsChannel");
        }
        
        foreach (YarnProgram prog in YarnProgramsChannel.DialoguePrograms)
        {
            NarrativeRunner.AddStringTable(prog);
        }

        NarrativeRunner.onDialogueComplete.AddListener(OnNarrativeComplete);
        NarrativeRunner.AddCommandHandler("ShowSprite", ShowSprite);
        NarrativeRunner.AddCommandHandler("ChangeScene", ChangeScene);
        NarrativeRunner.AddCommandHandler("Clear", Clear);
    }

    private void OnDestroy()
    {
        GameEventChannel.RemoveListener<NarrativeRequestedGEM>(OnNarrativeRequested);
        GameEventChannel.RemoveListener<GameQuitGEM>(OnGameQuit);
    }


    private void OnNarrativeComplete()
    {
        GameEventChannel.Broadcast(new NarrativeFinishedGEM());
    }

    private void OnNarrativeRequested(NarrativeRequestedGEM arg0)
    {
        NarrativeRunner.YarnScriptOnCompleteAction = arg0.OnComplete;
        NarrativeRunner.StartDialogue(arg0.NodeName);
        GameEventChannel.Broadcast(new NarrativeStartedGEM());
    }


    private void OnGameQuit(GameQuitGEM arg0)
    {
        NarrativeRunner.Stop();
    }

    private void ShowSprite(string[] parameters)
    {
        var newGO = Instantiate(SpritePanel, DialogueContainer.transform);
        var newPanel = newGO.GetComponent<SpritePanel>();
        var sprite = SpriteLibrary.GetSprite(parameters[0], parameters[1]);
        newPanel.Set(sprite);
    }

    private void ChangeScene(string[] parameters)
    {
        GameEventChannel.Broadcast(new TransitionGEM()
        {
            NewSceneName = parameters[0],
            TransitionOutEffect = TransitionEffectEnum.Fullscreen,
        });
    }

    private void Clear(string[] parameters)
    {
        var children = new List<GameObject>();

        foreach (Transform child in DialogueContainer.transform)
        {
            children.Add(child.gameObject);
        }

        children.ForEach(x => Destroy(x));
    }
}

public class NarrativeRequestedGEM : YarnArgs
{
    public string NodeName;
}

public class NarrativeStartedGEM : EventArgs
{

}

public class NarrativeFinishedGEM : EventArgs
{

}