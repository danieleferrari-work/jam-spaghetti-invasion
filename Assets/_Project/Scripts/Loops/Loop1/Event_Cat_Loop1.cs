using System.Collections;
using UnityEngine;

public class Event_Cat_Loop1 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayCatSound());
    }

    private IEnumerator PlayCatSound()
    {
        yield return new WaitForSeconds(Loop1.instance.catMeowStartDelay);

        for (int i = 0; i < Loop1.instance.catMeowsCount; i++)
        {
            AudioManager.instance.Play(Loop1.instance.catMeowClipName);
            yield return new WaitForSeconds(Loop1.instance.catMeowDelay);
        }
    }
}
