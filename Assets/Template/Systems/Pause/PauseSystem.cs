using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseSystem : MonoBehaviour
{
    public GameEventChannel GameEventChannel;

    public CanvasGroup PauseCanvasGroup;

    public float TimeToPause = 0.5f;
    public float TimeToUnpause = 0.25f;

    [ReadOnly] [SerializeField] PauseStateEnum pauseState = PauseStateEnum.Unpaused;



    // Start is called before the first frame update
    void Awake()
    {
        GameEventChannel.RegisterListener<RequestTogglePauseGEM>(OnRequestTogglePause);
    }

    private void OnRequestTogglePause(RequestTogglePauseGEM arg0)
    {
        switch (pauseState)
        {
            case PauseStateEnum.Unpaused:
                StartCoroutine(Pause());
                return;

            case PauseStateEnum.Pausing:
                return;

            case PauseStateEnum.Paused:
                StartCoroutine(Unpause());
                return;

            case PauseStateEnum.Unpausing:
                return;
        }
    }

    private IEnumerator Pause()
    {
        float timer = TimeToPause;
        pauseState = PauseStateEnum.Pausing;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp01(timer / TimeToPause);
            PauseCanvasGroup.alpha = 1 - Time.timeScale;
            yield return new WaitForEndOfFrame();
        }


        pauseState = PauseStateEnum.Paused;
        GameEventChannel.Broadcast(new GamePausedGEM());
    }

    private IEnumerator Unpause()
    {
        float timer = TimeToUnpause;
        pauseState = PauseStateEnum.Unpausing;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp01(1 - timer / TimeToUnpause);
            PauseCanvasGroup.alpha = 1 - Time.timeScale;
            yield return new WaitForEndOfFrame();
        }

        pauseState = PauseStateEnum.Unpaused;
        GameEventChannel.Broadcast(new GameUnpausedGEM());
    }
}

public enum PauseStateEnum
{
    Unpaused,
    Pausing,
    Paused,
    Unpausing,
}

public class RequestTogglePauseGEM : System.EventArgs
{

}

public class GamePausedGEM : System.EventArgs
{

}

public class GameUnpausedGEM : System.EventArgs
{

}
