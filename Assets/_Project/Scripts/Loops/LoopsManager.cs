using System.Collections.Generic;
using System.Linq;
using BaseTemplate;
using DevLocker.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoopsManager : Singleton<LoopsManager>
{
    [SerializeField] List<SceneReference> loops;

    int currentLoopIndex;

    ILoop currentLoop;

    public static UnityAction OnStartLoop;

    protected override bool isDontDestroyOnLoad => true;


    void Start()
    {
#if UNITY_EDITOR
        var loop = FindObjectsOfType<MonoBehaviour>().OfType<ILoop>().FirstOrDefault();
        if (loop != null)
        {
            currentLoop = loop;
            currentLoopIndex = loop.GetLoopNumber() - 1;
        }
#else
        currentLoopIndex = 0;
        LoadLoop(currentLoopIndex);
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartLoop();
    }

    public void OnLoopExit()
    {
        if (!currentLoop.IsComplete())
            RestartLoop();
        else
            NextLoop();
    }

    void RestartLoop()
    {
        UnloadLoop(currentLoopIndex);
        LoadLoop(currentLoopIndex);
    }

    void NextLoop()
    {
        UnloadLoop(currentLoopIndex);
        currentLoopIndex++;

        if (currentLoopIndex >= loops.Count)
        {
            Debug.Log("NO MORE LOOPS");
            currentLoopIndex = 0;
        }

        LoadLoop(currentLoopIndex);
    }

    void UnloadLoop(int index)
    {
        var unload = SceneManager.UnloadSceneAsync(loops[index].SceneName);
        unload.completed += OnUnloadComplete;
    }

    void OnUnloadComplete(AsyncOperation operation)
    {
        Debug.Log($"Unload scene completed");
    }

    void LoadLoop(int index)
    {
        var load = SceneManager.LoadSceneAsync(loops[index].SceneName, LoadSceneMode.Additive);
        load.completed += OnLoadComplete;
    }

    void OnLoadComplete(AsyncOperation operation)
    {
        OnStartLoop?.Invoke();
        currentLoop = FindObjectsOfType<MonoBehaviour>().OfType<ILoop>().First();
        Debug.Log($"Load scene completed");
    }
}
