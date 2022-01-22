using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOnLoad : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadFirstSceneAtGameBegins()
    {
        GameObject main = Instantiate(Resources.Load("GlobalSystems")) as GameObject;
        if (main == null)
        {
            Debug.LogError("Cannot find 'GlobalSystems' object in Resources");
        }

        DontDestroyOnLoad(main);

        main.transform.SetAsFirstSibling();

        GameObject ds = Instantiate(Resources.Load("DialogueSystem")) as GameObject;
        if (ds == null)
        {
            Debug.LogError("Cannot find 'DialoguelSystem' object in Resources");
        }

        DontDestroyOnLoad(ds);
    }
}


