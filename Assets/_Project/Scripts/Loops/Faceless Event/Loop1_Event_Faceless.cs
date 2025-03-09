using System;
using Cinemachine;
using UnityEngine;

public class Loop1_Event_Faceless : MonoBehaviour
{
    [SerializeField] GameObject firstFaceless;
    [SerializeField] GameObject secondFaceless;

    private ChangePovEffect firstFacelessChangePovEffect;
    private ChangePovEffect secondFacelessChangePovEffect;

    private WatchEvent firstFacelessWatchEvent;
    private WatchEvent secondFacelessWatchEvent;



    void Start()
    {
        firstFacelessChangePovEffect = firstFaceless.GetComponentInChildren<ChangePovEffect>();
        secondFacelessChangePovEffect = secondFaceless.GetComponentInChildren<ChangePovEffect>();

        firstFacelessWatchEvent = firstFaceless.GetComponentInChildren<WatchEvent>();
        secondFacelessWatchEvent = secondFaceless.GetComponentInChildren<WatchEvent>();

        firstFacelessWatchEvent.OnEventSuccessed += () => firstFacelessChangePovEffect.ChangeCamera();
        secondFacelessWatchEvent.OnEventSuccessed += () => secondFacelessChangePovEffect.ResetCamera();

        firstFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(firstFacelessChangePovEffect);
        secondFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(secondFacelessChangePovEffect);

        firstFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(firstFacelessChangePovEffect);
        secondFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(secondFacelessChangePovEffect);
    }

    private void OnStartWatchingFaceless(ChangePovEffect changePovEffect)
    {
        changePovEffect.hasSeenPlayer = true;
    }

    private void OnStopWatchingFaceless(ChangePovEffect changePovEffect)
    {
        changePovEffect.hasSeenPlayer = false;
    }
}
