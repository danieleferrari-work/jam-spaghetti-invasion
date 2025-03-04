using BaseTemplate;
using UnityEngine;

public class Loop1 : Singleton<Loop1>
{
    [Tooltip("Quanti secondi devono passare dal caricamento della scena al primo miagolio del gatto")]
    public float catMeowStartDelay;

    [Tooltip("Secondi tra un miagolio e l'altro")]
    public float catMeowDelay;

    [Tooltip("Quante volte il gatto miagola")]
    public float catMeowsCount;

    public string catMeowClipName;
}
