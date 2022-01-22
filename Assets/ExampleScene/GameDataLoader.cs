using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadFirstSceneAtGameBegins()
    {
        GameDataChannel.Instance.GameCreated += GameDataChannel_GameCreated;

        LoadData();
    }

    private static void GameDataChannel_GameCreated()
    {
        LoadData();
    }

    private static void LoadData()
    {
        Debug.Log("GameLoader Loading Default Data");
        // create game specific data in the GameDataChannel.Instance
    }
}


