using System.Collections.Generic;
using System.Linq;
using BaseTemplate;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private List<IPausable> pausableObjects = new List<IPausable>();
    private bool isPaused = false;

    public bool IsPaused => isPaused;

    protected override bool isDontDestroyOnLoad => false;


    protected override void InitializeInstance()
    {
        base.InitializeInstance();
        PlayerInputManager.instance.Pause += TogglePause;

        pausableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToList();
    }

    // Registers a Pausable object
    public void RegisterPausable(IPausable pausableObject)
    {
        if (!pausableObjects.Contains(pausableObject))
        {
            pausableObjects.Add(pausableObject);
        }
    }

    // Unregisters a Pausable object
    public void UnregisterPausable(IPausable pausableObject)
    {
        pausableObjects.Remove(pausableObject);
    }

    // Pauses all registered objects
    public void Pause()
    {
        if (isPaused)
            return;

        isPaused = true;
        foreach (var pausable in pausableObjects)
        {
            pausable.Pause();
        }
    }

    // Unpauses all registered objects
    public void Unpause()
    {
        if (!isPaused)
            return;

        isPaused = false;
        foreach (var pausable in pausableObjects)
        {
            pausable.Unpause();
        }
    }

    // Toggles between pause and unpause
    public void TogglePause()
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }

        pauseMenu.SetActive(isPaused);
    }
}
