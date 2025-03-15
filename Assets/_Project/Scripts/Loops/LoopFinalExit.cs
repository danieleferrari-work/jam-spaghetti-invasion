using UnityEngine;

public class LoopFinalExit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var gondola = other.GetComponent<Gondola>();

        if (gondola)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log($"END GAME");
    }
}
