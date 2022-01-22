using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Useable_Dialogue : Useable
{
    public GameEventChannel GameEventChannel;
    public string YarnDialogueNode;

    public override void Use()
    {
        GameEventChannel.Broadcast(new DialogueRequestedGEM()
        {
            NodeName = YarnDialogueNode,
        });
    }

}