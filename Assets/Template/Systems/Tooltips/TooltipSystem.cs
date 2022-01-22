using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TooltipSystem : MonoBehaviour
{
    public GameEventChannel GameEventChannel;
    public GameObject TooltipObject;
    public GameObject TooltipPrefab;
    public TooltipInfo CurrentTooltipInfo;

    // Start is called before the first frame update
    void Start()
    {
        GameEventChannel.RegisterListener<TooltipRequestGEM>(OnTooltipRequest);
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void OnTooltipRequest(TooltipRequestGEM arg0)
    {
        if (TooltipObject != null)
        {
            Destroy(TooltipObject);
            TooltipObject = null;
        }

        CurrentTooltipInfo = arg0.TooltipInfo;
        TooltipObject = Instantiate(TooltipPrefab, transform);
        var newDisplayer = TooltipObject.GetComponent<TooltipDisplayer>();
        newDisplayer.Set(arg0.TooltipInfo);
        arg0.TooltipInfo.PointerExitEvent += TooltipInfo_PointerExitEvent;        
    }

    private void TooltipInfo_PointerExitEvent()
    {
        CurrentTooltipInfo.PointerExitEvent -= TooltipInfo_PointerExitEvent;
        Destroy(TooltipObject);
        TooltipObject = null;
        CurrentTooltipInfo = null;
    }

    private void SceneManager_activeSceneChanged(Scene current, Scene next)
    {
        if (TooltipObject != null)
        {
            CurrentTooltipInfo.PointerExitEvent -= TooltipInfo_PointerExitEvent;
            Destroy(TooltipObject);
            TooltipObject = null;
            CurrentTooltipInfo = null;
        }
    }
}

public class TooltipRequestGEM : System.EventArgs
{
    public RectTransform RequestingRect;
    public TooltipInfo TooltipInfo;
}

