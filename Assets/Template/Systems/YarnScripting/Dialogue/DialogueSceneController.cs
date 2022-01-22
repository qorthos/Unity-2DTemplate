using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneController : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public GameDataChannel GameDataChannel;
    public string SceneStartNode;

    // Start is called before the first frame update
    void Start()
    {
        GameEventChannel.RegisterListener<DialogueStartedGEM>(OnDialogueStarted);
        GameEventChannel.RegisterListener<DialogueFinishedGEM>(OnDialogueFinished);

        GameEventChannel.Broadcast(new DialogueRequestedGEM()
        {
            NodeName = SceneStartNode,
        });
    }

    private void OnDestroy()
    {
        GameEventChannel.RemoveListener<DialogueStartedGEM>(OnDialogueStarted);
        GameEventChannel.RemoveListener<DialogueFinishedGEM>(OnDialogueFinished);
    }

    private void OnDialogueStarted(DialogueStartedGEM arg0)
    {

    }

    private void OnDialogueFinished(DialogueFinishedGEM arg0)
    {

    }
}
