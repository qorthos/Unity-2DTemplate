using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public delegate void TooltipInfoEvent();

public class TooltipInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameEventChannel GameEventChannel;
    public string DisplayText;
    public RectTransform RectTransform;

    public event TooltipInfoEvent PointerExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventChannel.Broadcast(new TooltipRequestGEM()
        {
            TooltipInfo = this,
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent?.Invoke();
    }
}
