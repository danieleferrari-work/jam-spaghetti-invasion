using System.Collections.Generic;
using System.Linq;
using BaseTemplate;
using DevLocker.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopsManager : Singleton<LoopsManager>
{
    [SerializeField] Gondola gondola;
    [SerializeField] List<SceneReference> loops;
    [SerializeField] int startingLoop;

    int currentLoopIndex;
    int previousLoop;
    ILoop currentLoop;

    protected override bool isDontDestroyOnLoad => false;

    void Start()
    {
        currentLoopIndex = startingLoop - 1;
        NextLoop();
    }

    public void OnLoopExit()
    {
        if (currentLoop != null && !currentLoop.IsComplete())
            LoadCurrentLoop();
        else
            NextLoop();
    }

    public void LoadCurrentLoop()
    {
        Debug.Log($"Start loading loop {currentLoopIndex} at {Time.time}");

        var loadScene = SceneManager.LoadSceneAsync(loops[currentLoopIndex].SceneName, LoadSceneMode.Additive);
        loadScene.completed += OnLoadingLoopCompleted;
    }

    public void NextLoop()
    {
        previousLoop = currentLoopIndex;
        currentLoopIndex++;

        LoadCurrentLoop();
    }

    private void OnLoadingLoopCompleted(AsyncOperation operation)
    {
        Debug.Log($"Finish loading loop {currentLoopIndex} at {Time.time}");
        Debug.Log($"Start unloading loop {previousLoop} at {Time.time}");

        currentLoop = FindObjectsOfType<MonoBehaviour>().OfType<ILoop>().First();

        gondola.OnLoopReset();

        if (previousLoop != -1)
        {
            var unloadScene = SceneManager.UnloadSceneAsync(loops[previousLoop].SceneName);
            unloadScene.completed += OnUnloadingPreviousLoopCompleted;
        }
    }

    private void OnUnloadingPreviousLoopCompleted(AsyncOperation operation)
    {
        Debug.Log($"Finish loading loop {currentLoopIndex} at {Time.time}");
    }
}
