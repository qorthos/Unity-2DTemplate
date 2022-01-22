using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NarrativePanel : MonoBehaviour
{
    public Image PortraitImage;
    public TMPro.TMP_Text NameText;
    public TMPro.TMP_Text LineText;



    public void Set(Sprite portraitSprite, string characterName, string lineText)
    {
        if (PortraitImage != null) PortraitImage.sprite = portraitSprite;
        if (NameText != null) NameText.text = characterName;
        LineText.text = lineText;
    }

}
