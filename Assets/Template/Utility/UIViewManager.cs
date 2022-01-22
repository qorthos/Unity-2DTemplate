using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewManager : MonoBehaviour
{
    [ReadOnly] [SerializeField] UIViewController activeView;
    [ReadOnly][SerializeField] UIViewController nextView;
    [ReadOnly] [SerializeField] bool isBlocked = false;


    public void SetInitial(UIViewController initialView)
    {
        activeView = initialView;
    }

    public void ChangeView(UIViewController next)
    {
        if (isBlocked)
            return;

        if (activeView == next)
            return;

        isBlocked = true;
        nextView = next;
        StartCoroutine(ChangeViews());
    }

    private IEnumerator ChangeViews()
    {
        if (activeView != null)
        {
            activeView.Hide();
            activeView = null;
            //yield return new WaitForSeconds(1.0f);
        }

        if (nextView != null)
        {
            activeView = nextView;
            nextView = null;
            activeView.Show();

            //yield return new WaitForSeconds(1.0f);
        }

        isBlocked = false;
        yield return null;
    }
}
