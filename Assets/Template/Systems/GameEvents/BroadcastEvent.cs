using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BroadcastEvent<T> : UnityEvent<T>, IBroadcastEvent where T : System.EventArgs
{
    public void Clear()
    {
        RemoveAllListeners();
    }
}

public interface IBroadcastEvent
{
    void Clear();
}