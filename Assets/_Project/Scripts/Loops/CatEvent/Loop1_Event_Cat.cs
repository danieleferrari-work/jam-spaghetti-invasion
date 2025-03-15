using System.Collections;
using UnityEngine;

public class Loop1_Event_Cat : MonoBehaviour
{
    [SerializeField] AudioSource3D audioCatMeowing;

    void Start()
    {
        StartCoroutine(PlayMeowing());
    }

    IEnumerator PlayMeowing()
    {
        while (true)
        {
            var randomDelay = Random.Range(Params.instance.loop1_catMeowingMinDelay, Params.instance.loop1_catMeowingMaxDelay);
            
            yield return new WaitForSeconds(randomDelay);

            audioCatMeowing.Play();
        }
    }
}
