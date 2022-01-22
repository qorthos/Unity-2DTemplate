using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplayer : MonoBehaviour
{
    public GameDataChannel GameDataChannel;
    public string Variable;
    public string Prefix;
    public string Suffix;

    public bool HideIfBlank = false;

    public TMPro.TMP_Text Text;

    // Start is called before the first frame update
    void Start()
    {
        GameDataChannel.VariableChanged += YarnMemoryChannel_VariableChanged;
        UpdateText(GameDataChannel.GetValue(Variable).AsString);
    }

    private void YarnMemoryChannel_VariableChanged(string varName, Yarn.Value newVal)
    {
        if (varName == Variable)
            UpdateText(newVal.AsString);
    }

    void UpdateText(string val)
    {
        Text.text = $"{Prefix}{val}{Suffix}";
        if (val.Equals("") && HideIfBlank)
        {
            Text.enabled = false;
        }
        else
        {
            Text.enabled = true;
        }
    }

    private void OnDestroy()
    {
        GameDataChannel.VariableChanged -= YarnMemoryChannel_VariableChanged;
    }
}
