using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpritePanel : MonoBehaviour
{
    public Image Image;


    public void Set(Sprite sprite)
    {
        if (Image != null) Image.sprite = sprite;
    }

}
