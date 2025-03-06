using System.Collections;
using UnityEngine;

public class Loop2_Event_Cat : MonoBehaviour
{
    Loop2 loop;

    void Awake()
    {
        loop = GetComponentInParent<Loop2>();
    }

    void Start()
    {
        StartCoroutine(PlayCatSound());
    }
    
    private IEnumerator PlayCatSound()
    {
        yield return new WaitForSeconds(loop.catMeowStartDelay);

        for (int i = 0; i < loop.catMeowsCount; i++)
        {
            AudioManager.instance.Play(loop.catMeowClipName);
            yield return new WaitForSeconds(loop.catMeowDelay);
        }
    }
}
