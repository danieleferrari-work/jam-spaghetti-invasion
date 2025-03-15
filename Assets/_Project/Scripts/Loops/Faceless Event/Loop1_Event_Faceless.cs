using Cinemachine;
using UnityEngine;

public class Loop1_Event_Faceless : MonoBehaviour
{
    [SerializeField] GameObject firstFaceless;
    [SerializeField] GameObject secondFaceless;

    private ChangePovEffect changePovEffect;

    private CinemachineVirtualCamera firstFacelessCamera;

    private WatchEvent firstFacelessWatchEvent;
    private WatchEvent secondFacelessWatchEvent;
    private Loop1 loop;


    void Awake()
    {
        loop = GetComponentInParent<Loop1>();

        secondFaceless.SetActive(false);

        changePovEffect = GetComponentInChildren<ChangePovEffect>();

        firstFacelessWatchEvent = firstFaceless.GetComponentInChildren<WatchEvent>();
        secondFacelessWatchEvent = secondFaceless.GetComponentInChildren<WatchEvent>();

        firstFacelessCamera = firstFaceless.GetComponentInChildren<CinemachineVirtualCamera>(true);
    }

    void Start()
    {
        firstFacelessWatchEvent.OnEventSuccessed += () => OnFirstFacelessEventSuccess();
        secondFacelessWatchEvent.OnEventSuccessed += () => OnSecondFacelessEventSuccess();

        firstFacelessWatchEvent.OnStartWatching += () => changePovEffect.StartShaking();
        secondFacelessWatchEvent.OnStartWatching += () => changePovEffect.StartShaking();

        firstFacelessWatchEvent.OnStopWatching += () => changePovEffect.StopShaking();
        secondFacelessWatchEvent.OnStopWatching += () => changePovEffect.StopShaking();
    }

    private void OnFirstFacelessEventSuccess()
    {
        //changePovEffect.ChangeCamera();
        secondFaceless.SetActive(true);
        changePovEffect.StopShaking();
        changePovEffect.ChangeCamera(firstFacelessCamera);
    }

    private void OnSecondFacelessEventSuccess()
    {
        changePovEffect.StopShaking();
        changePovEffect.ResetCamera();

        loop.facelessEventCompleted = true;
    }
}
