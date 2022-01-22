using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "GameEventChannel", menuName = "Channels/GameEventChannel")]
public class GameEventChannel : ScriptableObject
{
    public static GameEventChannel Instance;

    private Dictionary<Type, IBroadcastEvent> broadcastEventLib;

    private void OnEnable()
    {
        Instance = this;
        broadcastEventLib = new Dictionary<Type, IBroadcastEvent>();
    }

    private void OnDisable()
    {
        Instance = null;
        foreach (IBroadcastEvent broadcastEvent in broadcastEventLib.Values)
        {
            broadcastEvent.Clear();
        }

        broadcastEventLib.Clear();
    }

    public void Broadcast<T>(T args) where T : EventArgs
    {
        var type = typeof(T);

        Debug.Log($"Broadcasting: {type}");
        if (broadcastEventLib.ContainsKey(type) == false)
        {
            broadcastEventLib.Add(type, new BroadcastEvent<T>());
        }

        ((BroadcastEvent<T>)broadcastEventLib[type]).Invoke(args);
    }

    public void RegisterListener<T>(UnityAction<T> callback) where T : EventArgs
    {
        var type = typeof(T);

        if (broadcastEventLib.ContainsKey(type) == false)
        {
            broadcastEventLib.Add(type, new BroadcastEvent<T>());
        }

        ((BroadcastEvent<T>)broadcastEventLib[type]).AddListener(callback);
    }

    public void RemoveListener<T>(UnityAction<T> callback) where T : EventArgs
    {
        var type = typeof(T);

        if (broadcastEventLib.ContainsKey(type) == false)
        {
            Debug.LogError("Attempting to remove a listener from a BroadcastEvent that never existed in the first place?!");
            return;
        }

        ((BroadcastEvent<T>)broadcastEventLib[type]).RemoveListener(callback);
    }

}
