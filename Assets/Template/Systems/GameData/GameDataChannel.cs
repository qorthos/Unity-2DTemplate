using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Yarn;

public delegate void GameDataChannelEvent();
public delegate void VariableChangedEvent(string varName, Value newVal);

[CreateAssetMenu(fileName = "GameDataChannel", menuName = "Channels/GameDataChannel")]
public class GameDataChannel : VariableStorageBehaviour
{
    [NonSerialized] public static GameDataChannel Instance;

    public string Slot0Time;
    public string Slot1Time;
    public string Slot2Time;
    public int CurrentSlot;

    public GameData GameData;

    public List<DefaultVariable> DefaultVariables;

    public event GameDataChannelEvent GameClearing;
    public event GameDataChannelEvent GameCreated;
    public event GameDataChannelEvent GameLoaded;
    public event GameDataChannelEvent GameSaved;

    public event VariableChangedEvent VariableChanged;

    private void OnEnable()
    {
        GameClearing = null;
        GameCreated = null;
        GameLoaded = null;
        GameSaved = null;

        Instance = this;

        Slot0Time = PlayerPrefs.GetString("Slot0Time", "Empty");
        Slot1Time = PlayerPrefs.GetString("Slot1Time", "Empty");
        Slot2Time = PlayerPrefs.GetString("Slot2Time", "Empty");

        ResetToDefaults();
    }

    void OnDisable()
    {
        GameClearing = null;
        GameCreated = null;
        GameLoaded = null;
        GameSaved = null;

        GameData = null;

        Instance = null;
    }

    /// <summary>
    /// Removes all variables, and replaces them with the variables
    /// defined in <see cref="DefaultVariables"/>.
    /// </summary>
    public override void ResetToDefaults()
    {
        Clear();

        // For each default variable that's been defined, parse the
        // string that the user typed in in Unity and store the
        // variable
        foreach (var variable in DefaultVariables)
        {
            object value;

            switch (variable.type)
            {
                case Yarn.Value.Type.Number:
                    float f = 0.0f;
                    float.TryParse(variable.value, out f);
                    value = f;
                    break;

                case Yarn.Value.Type.String:
                    value = variable.value;
                    break;

                case Yarn.Value.Type.Bool:
                    bool b = false;
                    bool.TryParse(variable.value, out b);
                    value = b;
                    break;

                case Yarn.Value.Type.Variable:
                    // We don't support assigning default variables from
                    // other variables yet
                    Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                        "set a default variable to be another variable, because it " +
                        "may not have been initialised yet.", variable.name, variable.value);
                    continue;

                case Yarn.Value.Type.Null:
                    value = null;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException();

            }

            var v = new Yarn.Value(value);

            SetValue(variable.name, v);
        }
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        Slot0Time = PlayerPrefs.GetString("Slot0Time", "Empty");
        Slot1Time = PlayerPrefs.GetString("Slot1Time", "Empty");
        Slot2Time = PlayerPrefs.GetString("Slot2Time", "Empty");
    }

    public void SaveToPrefs() => SaveToPrefs(CurrentSlot);

    public void SaveToPrefs(int slot)
    {
        CurrentSlot = slot;
        var saveString = JsonConvert.SerializeObject(GameData);
        PlayerPrefs.SetString($"Slot{slot}", saveString);
        PlayerPrefs.Save();

        PlayerPrefs.SetString($"Slot{slot}Time", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

        Slot0Time = PlayerPrefs.GetString("Slot0Time", "Empty");
        Slot1Time = PlayerPrefs.GetString("Slot1Time", "Empty");
        Slot2Time = PlayerPrefs.GetString("Slot2Time", "Empty");

        Debug.Log("GameDataChannel: Saved");
        GameSaved?.Invoke();
    }

    public void LoadFromPrefs() => LoadFromPrefs(CurrentSlot);

    public void LoadFromPrefs(int slot)
    {
        CurrentSlot = slot;
        var saveGameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString($"Slot{CurrentSlot}"));
        GameData = saveGameData;

        PlayerPrefs.SetString($"Slot{slot}Time", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
        Slot0Time = PlayerPrefs.GetString("Slot0Time", "Empty");
        Slot1Time = PlayerPrefs.GetString("Slot1Time", "Empty");
        Slot2Time = PlayerPrefs.GetString("Slot2Time", "Empty");

        Debug.Log("GameDataChannel: Loaded");
        GameLoaded?.Invoke();
    }

    public override Value GetValue(string variableName)
    {
        var split = variableName.Split(new[] { '_' }, 2);

        if (split.Length > 1)
        {
            if (GameData.Containers.ContainsKey(split[0]))
            {
                return GameData.Containers[split[0]].GetValue(split[1]);
            }
            else
            {
                Debug.LogError($"Could not find yarn variable: {variableName}, no container: {split[0]}");
                return Yarn.Value.NULL;
            }
        }
        else
        {
            // If we don't have a variable with this name, return the null
            // value
            if (GameData.Variables.ContainsKey(variableName) == false)
            {
                Debug.LogWarning($"Could not find yarn variable: {variableName}");
                return Yarn.Value.NULL;
            }

            return GameData.Variables[variableName];
        }
    }

    public override void SetValue(string variableName, Value value)
    {
        var split = variableName.Split(new[] { '_' }, 2);

        if (split.Length > 1)
        {
            if (GameData.Containers.ContainsKey(split[0]))
            {
                GameData.Containers[split[0]].SetValue(split[1], value);
                VariableChanged?.Invoke(variableName, value);
            }
            else
            {
                Debug.LogError($"Could not find yarn variable: {variableName}, no container: {split[0]}");
            }
        }
        else
        {

            if (GameData.Variables.ContainsKey(variableName) == false)
            {
                GameData.Variables.Add(variableName, value);
                VariableChanged?.Invoke(variableName, value);
            }
            else
            {
                GameData.Variables[variableName] = value;
                VariableChanged?.Invoke(variableName, value);
            }
        }
    }

    public override void Clear()
    {
        Debug.Log("GameDataChannel: Clearing");
        GameClearing?.Invoke();
        GameData = new GameData();
        Debug.Log("GameDataChannel: Created");
        GameCreated?.Invoke();
    }

    public void StartNewGame(int slot)
    {
        ResetToDefaults();
        SaveToPrefs(slot);
    }
}

/// <summary>
/// A default value to apply when the object wakes up, or when
/// ResetToDefaults is called.
/// </summary>
[System.Serializable]
public class DefaultVariable
{
    /// <summary>
    /// The name of the variable.
    /// </summary>
    /// <remarks>
    /// Do not include the `$` prefix in front of the variable
    /// name. It will be added for you.
    /// </remarks>
    public string name;

    /// <summary>
    /// The value of the variable, as a string.
    /// </summary>
    /// <remarks>
    /// This string will be converted to the appropriate type,
    /// depending on the value of <see cref="type"/>.
    /// </remarks>
    public string value;

    /// <summary>
    /// The type of the variable.
    /// </summary>
    public Yarn.Value.Type type;

    [ReadOnly] public bool IsCodeGenerated = false;
}