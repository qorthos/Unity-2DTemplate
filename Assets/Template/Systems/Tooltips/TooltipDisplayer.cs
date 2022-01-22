using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipDisplayer : MonoBehaviour
{
    public TMPro.TMP_Text Text;
    public RectTransform RectTransform;
    public RectTransform BackgroundTransform;
    public RectTransform TextTransform;
    TooltipInfo info;

    public void Set(TooltipInfo info)
    {
        this.info = info;
        Text.text = info.DisplayText;

        Vector2 mousePos = Mouse.current.position.ReadDefaultValue();
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        var xanchor = 0;
        var yanchor = 0;
        if (mousePos.x > screenCenter.x)
        {
            xanchor = 1;
        }
        if (mousePos.y > screenCenter.y)
        {
            yanchor = 1;
        }

        RectTransform.pivot = new Vector2(xanchor, yanchor);
        RectTransform.anchoredPosition = mousePos - screenCenter;
        BackgroundTransform.pivot = RectTransform.pivot;
        TextTransform.pivot = RectTransform.pivot;
    }

    private void Update()
    {
        BackgroundTransform.sizeDelta = TextTransform.sizeDelta;
    }

}