using System.Collections;
using UnityEngine;

public class Loop1 : MonoBehaviour
{
    [Tooltip("Quanti secondi devono passare dal caricamento della scena al primo miagolio del gatto")]
    [SerializeField] float catMeowStartDelay;

    [Tooltip("Secondi tra un miagolio e l'altro")]
    [SerializeField] float catMeowDelay;

    [Tooltip("Quante volte il gatto miagola")]
    [SerializeField] float catMeowsCount;

    [SerializeField] string catMeowClipName;


    void Start()
    {
        StartCoroutine(PlayCatSound());
    }

    private IEnumerator PlayCatSound()
    {
        yield return new WaitForSeconds(catMeowStartDelay);

        for (int i = 0; i < catMeowsCount; i++)
        {
            AudioManager.instance.Play(catMeowClipName);
            yield return new WaitForSeconds(catMeowDelay);
        }
    }
}
