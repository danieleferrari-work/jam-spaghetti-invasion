using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void SetHorSensibility(float sliderValue)
    {
        PlayerPrefs.SetFloat("horizontalSensibility", Mathf.Log10(sliderValue) * 100);
    }

    public void SetVerSensibility(float sliderValue)
    {
        PlayerPrefs.SetFloat("verticalSensibility", Mathf.Log10(sliderValue) * 100);
    }
}
