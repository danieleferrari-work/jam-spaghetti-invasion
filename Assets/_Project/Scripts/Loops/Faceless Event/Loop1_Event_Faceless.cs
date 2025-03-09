using System;
using Cinemachine;
using UnityEngine;

public class Loop1_Event_Faceless : MonoBehaviour
{
    [SerializeField] GameObject firstFaceless;
    [SerializeField] GameObject secondFaceless;

    private ShakingEffect firstFacelessShakingEffect;
    private ShakingEffect secondFacelessShakingEffect;

    private WatchEvent firstFacelessWatchEvent;
    private WatchEvent secondFacelessWatchEvent;



    void Start()
    {
        firstFacelessShakingEffect = firstFaceless.GetComponentInChildren<ShakingEffect>();
        secondFacelessShakingEffect = secondFaceless.GetComponentInChildren<ShakingEffect>();

        firstFacelessWatchEvent = firstFaceless.GetComponentInChildren<WatchEvent>();
        secondFacelessWatchEvent = secondFaceless.GetComponentInChildren<WatchEvent>();

        firstFacelessWatchEvent.OnEventSuccessed += () => OnFacelessEventSuccess(firstFacelessShakingEffect, false);
        secondFacelessWatchEvent.OnEventSuccessed += () => OnFacelessEventSuccess(secondFacelessShakingEffect, true);

        firstFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(firstFacelessShakingEffect);
        secondFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(secondFacelessShakingEffect);

        firstFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(firstFacelessShakingEffect);
        secondFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(secondFacelessShakingEffect);
    }



    private void OnFacelessEventSuccess(ShakingEffect shakingEffect, bool isLastFaceless)
    {
        if (isLastFaceless)
        {
            shakingEffect.ResetCamera();
        }
        else
        {
            shakingEffect.ChangeCamera();
        }

        shakingEffect.StopShaking();
    }

    private void OnStartWatchingFaceless(ShakingEffect ShakingEffect)
    {
        ShakingEffect.StartShaking();
    }

    private void OnStopWatchingFaceless(ShakingEffect ShakingEffect)
    {
        ShakingEffect.StopShaking();
    }
}
