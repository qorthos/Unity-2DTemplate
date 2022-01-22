using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class DialogueOptions : MonoBehaviour
{
    public List<Button> Options;
    public TMPro.TMP_Text Caption;

    public void SetCaption(string text)
    {
        Caption.gameObject.SetActive(true);
        Caption.text = text;
    }

}