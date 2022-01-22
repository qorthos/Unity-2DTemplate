using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

public class SimpleDataContainer : GameDataContainer
{
    Dictionary<string, Value> lib = new Dictionary<string, Value>();

    public override Value GetValue(string variableName)
    {
        if (!lib.ContainsKey(variableName))
        {
            lib.Add(variableName, new Value());
            Debug.LogWarning($"No such variable to get: {variableName}, creating null");
        }

        return lib[variableName];
    }

    public override void SetValue(string variableName, Value value)
    {
        if (!lib.ContainsKey(variableName))
        {
            lib.Add(variableName, new Value());
            Debug.LogWarning($"No such variable to set: {variableName}, creating null");
        }

        lib[variableName] = value;
        Notify(variableName);
    }

}
