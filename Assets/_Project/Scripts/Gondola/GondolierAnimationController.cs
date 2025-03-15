using UnityEngine;

public class GondolierAnimationController : MonoBehaviour
{
    [SerializeField] AudioSource3D rowingSound;

    public void OnRowTouchesWater()
    {
        rowingSound.Play();
    }
}
