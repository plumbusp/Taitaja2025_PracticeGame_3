using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1.0f;
        AudioManager.instance.PlayAudio(MusicType.GroovyMenuMusic); 
    }

    public void StartGame()
    {
        AudioManager.instance.PlayAudio(SFXType.ClickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
