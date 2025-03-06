using System.Collections;
using UnityEngine;

public class GondolierSing : MonoBehaviour
{
    Loop1 loop;

    void Awake()
    {
        loop = GetComponentInParent<Loop1>();
    }

    void Start()
    {
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(loop.gondolierSingStartDelay);

        for (int i = 0; i < loop.gondolierSingCount; i++)
        {
            AudioManager.instance.Play(loop.gondolierSingClipName);
            yield return new WaitForSeconds(loop.gondolierSingDelay);
        }
    }
}


