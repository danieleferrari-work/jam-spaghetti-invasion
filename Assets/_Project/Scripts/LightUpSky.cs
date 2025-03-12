using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class LightUpSky : MonoBehaviour
{
    [SerializeField] private Volume volume;
    public bool hasToLightUp;
    [SerializeField] private Animator skyAnim;
    [SerializeField] private float timerBloom, timerExposure;
    [SerializeField] private float maxBloom, maxExposure;
    private Bloom bloom;
    private Exposure exposure;

    // Start is called before the first frame update
    void Start()
    {
        if (volume.profile.TryGet(out bloom))
        {
            Debug.Log("Bloom found!");
        }
        if (volume.profile.TryGet(out exposure))
        {
            Debug.Log("Exposure found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasToLightUp) 
        {
            skyAnim.SetBool("HasToLightUp", true);
            if (bloom.intensity.value < maxBloom)
                bloom.intensity.value += timerBloom * Time.deltaTime;
            else
                bloom.intensity.value = maxBloom;
            if (exposure.fixedExposure.value > maxExposure)
                exposure.fixedExposure.value -= timerExposure * Time.deltaTime;
            else
                exposure.fixedExposure.value = maxExposure;
        }
    }
}
