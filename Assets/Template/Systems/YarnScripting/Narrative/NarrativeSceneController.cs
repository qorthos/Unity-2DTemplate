using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NarrativeSceneController : MonoBehaviour
{    
    public GameEventChannel GameEventChannel;
    public GameDataChannel GameDataChannel;
    public string SceneStartNode;

    // Start is called before the first frame update
    void Start()
    {
        SceneStartNode = GameDataChannel.GetValue("$NarrativeNode").AsString;

        GameEventChannel.RegisterListener<NarrativeStartedGEM>(OnNarrativeStarted);
        GameEventChannel.RegisterListener<NarrativeFinishedGEM>(OnNarrativeFinished);

        GameEventChannel.Broadcast(new NarrativeRequestedGEM()
        {
            NodeName = SceneStartNode,
        });
    }

    private void OnDestroy()
    {
        GameEventChannel.RemoveListener<NarrativeStartedGEM>(OnNarrativeStarted);
        GameEventChannel.RemoveListener<NarrativeFinishedGEM>(OnNarrativeFinished);
    }

    private void OnNarrativeStarted(NarrativeStartedGEM arg0)
    {

    }

    private void OnNarrativeFinished(NarrativeFinishedGEM arg0)
    {

    }
}
