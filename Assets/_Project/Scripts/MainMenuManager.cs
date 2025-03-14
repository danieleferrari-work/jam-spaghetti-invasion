using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public float hor_sensibility;
    public float ver_sensibility;

    public void OnClickPlay()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void SetHorSensibility (float sliderValue)
    {
        hor_sensibility = Mathf.Log10(sliderValue) * 100;
    }

    public void SetVerSensibility (float sliderValue)
    {
        ver_sensibility = Mathf.Log10(sliderValue) * 100;
    }
}
