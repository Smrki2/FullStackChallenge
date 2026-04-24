using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour 
{
    public void OnStartClicked()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void OnExitClicked()
    {
        Application.Quit();
    }
}
