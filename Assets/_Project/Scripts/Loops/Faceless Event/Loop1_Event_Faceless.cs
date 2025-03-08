using System;
using UnityEngine;

public class Loop1_Event_Faceless : MonoBehaviour
{
    [SerializeField] WatchEvent watchEvent;
    [SerializeField] ChangePovEffect changePovEffect;


    void Start()
    {
        watchEvent.OnStartWatching += OnStartWatchingFaceless;
        watchEvent.OnStopWatching += OnStopWatchingFaceless;
        watchEvent.OnEventSuccessed += OnEventSuccess;
    }

    private void OnEventSuccess()
    {
        changePovEffect.ChangeCamera();
    }

    private void OnStartWatchingFaceless()
    {
        changePovEffect.hasSeenPlayer = true;
    }

    private void OnStopWatchingFaceless()
    {
        changePovEffect.hasSeenPlayer = false;
    }
}
