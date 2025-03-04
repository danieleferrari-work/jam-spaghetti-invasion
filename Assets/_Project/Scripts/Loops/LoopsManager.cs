using System.Collections.Generic;
using BaseTemplate;
using DevLocker.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopsManager : Singleton<LoopsManager>
{
    [SerializeField] Gondola gondola;
    [SerializeField] List<SceneReference> loops;
    [SerializeField] int startingLoop;

    int currentLoop;
    int previousLoop;

    protected override bool isDontDestroyOnLoad => false;

    void Start()
    {
        currentLoop = startingLoop - 1;
        NextLoop();
    }

    public void NextLoop()
    {
        previousLoop = currentLoop;
        currentLoop++;

        Debug.Log($"Start loading loop {currentLoop} at {Time.time}");

        var loadScene = SceneManager.LoadSceneAsync(loops[currentLoop].SceneName, LoadSceneMode.Additive);
        loadScene.completed += OnLoadingLoopCompleted;
    }

    private void OnLoadingLoopCompleted(AsyncOperation operation)
    {
        Debug.Log($"Finish loading loop {currentLoop} at {Time.time}");
        Debug.Log($"Start unloading loop {previousLoop} at {Time.time}");

        gondola.OnLoopReset();

        if (previousLoop != -1)
        {
            var unloadScene = SceneManager.UnloadSceneAsync(loops[previousLoop].SceneName);
            unloadScene.completed += OnUnloadingPreviousLoopCompleted;
        }
    }

    private void OnUnloadingPreviousLoopCompleted(AsyncOperation operation)
    {
        Debug.Log($"Finish loading loop {currentLoop} at {Time.time}");
    }
}
