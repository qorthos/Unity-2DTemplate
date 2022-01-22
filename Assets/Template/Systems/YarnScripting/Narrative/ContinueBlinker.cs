using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueBlinker : MonoBehaviour
{
    public TMPro.TMP_Text Text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Blink));
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            Text.text = ".";
            yield return new WaitForSeconds(0.3f);
            Text.text = "..";
            yield return new WaitForSeconds(0.3f);
            Text.text = "...";
        }
    }
}
