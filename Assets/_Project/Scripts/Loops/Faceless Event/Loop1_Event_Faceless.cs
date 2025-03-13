using UnityEngine;

public class Loop1_Event_Faceless : MonoBehaviour
{
    [SerializeField] GameObject firstFaceless;
    [SerializeField] GameObject secondFaceless;

    private ChangePovEffect firstFacelessChangePovEffect;
    private ChangePovEffect secondFacelessChangePovEffect;

    private WatchEvent firstFacelessWatchEvent;
    private WatchEvent secondFacelessWatchEvent;
    private Loop1 loop;


    void Start()
    {
        loop = GetComponentInParent<Loop1>();

        secondFaceless.gameObject.SetActive(false);

        firstFacelessChangePovEffect = firstFaceless.GetComponentInChildren<ChangePovEffect>();
        secondFacelessChangePovEffect = secondFaceless.GetComponentInChildren<ChangePovEffect>();

        firstFacelessWatchEvent = firstFaceless.GetComponentInChildren<WatchEvent>();
        secondFacelessWatchEvent = secondFaceless.GetComponentInChildren<WatchEvent>();

        firstFacelessWatchEvent.OnEventSuccessed += () => OnFirstFacelessEventSuccess(firstFacelessChangePovEffect);
        secondFacelessWatchEvent.OnEventSuccessed += () => OnSecondFacelessEventSuccess(secondFacelessChangePovEffect);

        firstFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(firstFacelessChangePovEffect);
        secondFacelessWatchEvent.OnStartWatching += () => OnStartWatchingFaceless(secondFacelessChangePovEffect);

        firstFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(firstFacelessChangePovEffect);
        secondFacelessWatchEvent.OnStopWatching += () => OnStopWatchingFaceless(secondFacelessChangePovEffect);
    }



    private void OnFirstFacelessEventSuccess(ChangePovEffect changePovEffect)
    {
        changePovEffect.ChangeCamera();
        secondFaceless.SetActive(true);
        changePovEffect.StopShaking();
    }

    private void OnSecondFacelessEventSuccess(ChangePovEffect changePovEffect)
    {
        changePovEffect.ResetCamera();
        changePovEffect.StopShaking();

        loop.facelessEventCompleted = true;
    }

    private void OnStartWatchingFaceless(ChangePovEffect changePovEffect)
    {
        changePovEffect.StartShaking();
    }

    private void OnStopWatchingFaceless(ChangePovEffect changePovEffect)
    {
        changePovEffect.StopShaking();
    }
}
