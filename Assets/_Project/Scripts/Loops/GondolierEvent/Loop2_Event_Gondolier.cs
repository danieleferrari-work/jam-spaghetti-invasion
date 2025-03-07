using System.Collections;
using UnityEngine;

public class Loop2_Event_Gondolier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WatchEvent watchEvent;
  //  [SerializeField] Animator gondolierAnimator;
   // [SerializeField] CatJumpStateMachine gondolierJumpStateMachine;

    // References
    Loop2 loop;

    // Local Variables
    Coroutine singingGondolierCoroutine;
    bool isLastJump;

    void Awake()
    {
        loop = GetComponentInParent<Loop2>();

        if (loop.gondolierEventCompleted)
        {
            Destroy(gameObject);
        }

        watchEvent.OnEventStarted += StartSinging;
        watchEvent.OnEventSuccessed += StopSinging;
    }
    private void Start()
    {
        AudioManager.instance.Play(loop.gondolierSingClipName);
    }

    void StartSinging()
    {
        //  singingGondolierCoroutine = StartCoroutine(PlayGondolierAnimation());
        
    }

   void StopSinging()
    {
        AudioManager.instance.StopPlaying(loop.gondolierSingClipName);
        loop.gondolierEventCompleted = true;
        Destroy(gameObject);
    }

   /*
    public void CatJumpFinished()
    {
        if (isLastJump)
        {
            loop.gondolierEventCompleted = true;
            Destroy(gameObject);
        }
    }

     IEnumerator PlayGondolierAnimation()
    {
        for (int i = 0; i < loop.catJumpRepetitions; i++)
        {
            yield return new WaitForSeconds(loop.catJumpPause);

            catAnimator.SetTrigger("DoJump");
        }
    }
  */
}