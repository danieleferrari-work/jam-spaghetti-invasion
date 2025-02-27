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
}
