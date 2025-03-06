using UnityEngine;

public class ExamplePausableObject : MonoBehaviour, IPausable
{
    private void Update()
    {
        if (PauseManager.instance.IsPaused)
            return;
    }


    private void FixedUpdate()
    {
        if (PauseManager.instance.IsPaused)
            return;
    }

    // Pause object-related actions
    public void Pause()
    {
        // Example: Pause animations, if applicable
        // animator.speed = 0f;
    }

    // Resume object-related actions
    public void Unpause()
    {
        // Example: Resume animations
        // animator.speed = 1f;
    }
}

