using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIViewController : MonoBehaviour, IShowable
{
    public bool StartHidden = true;

    public bool IsShown => throw new System.NotImplementedException();

    void Start()
    {
        var rect = GetComponent<RectTransform>();

        if (StartHidden)
        {
            transform.position = transform.position + new Vector3(rect.rect.width, 0, 0);
        }
        else
        {
            FindObjectOfType<UIViewManager>().SetInitial(this);
        }
    }


    public void Hide()
    {
        var rect = GetComponent<RectTransform>();

        transform.DOMove(transform.position + new Vector3(rect.rect.width, 0, 0), 0.33f)
            .SetEase(Ease.InOutQuad);
    }

    public void Show()
    {
        var rect = GetComponent<RectTransform>();

        transform.position = transform.position - 2 * new Vector3(rect.rect.width, 0, 0);

        transform.DOMove(transform.position + new Vector3(rect.rect.width, 0, 0), 0.33f)
            .SetEase(Ease.InOutQuad);
    }
}
