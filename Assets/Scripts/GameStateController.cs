using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private WinStateTimer _WinStateTimer;
    [SerializeField] private HealthController _HealthController;

    [SerializeField] private GameObject _DeadScreen;
    [SerializeField] private GameObject _WinScreen;
    [SerializeField] private GameObject _PlayModeUI;

    private void Awake()
    {
        _PlayModeUI.SetActive(true);
        _WinScreen.SetActive(false);
        _DeadScreen.SetActive(false);

        _WinStateTimer.OnWin += ShowWinScreen;
        _HealthController.OnDead += ShowDeadScreen;  

        Time.timeScale = 1f;
    }

    private void ShowDeadScreen()
    {
        _PlayModeUI.SetActive(false);
        _DeadScreen.SetActive(true);
        PuzzleManager.Instance.ClosePuzzle();
        AudioManager.instance.PlayAudio(SFXType.BigLoseTime);
        Time.timeScale = 0f;
    }
    private void ShowWinScreen()
    {
        _PlayModeUI.SetActive(false);
        _WinScreen.SetActive(true);
        PuzzleManager.Instance.ClosePuzzle();
        AudioManager.instance.PlayAudio(SFXType.BigWinSound);
        Time.timeScale = 0f;
    }

    public void BackToMenu()
    {
        AudioManager.instance.PlayAudio(SFXType.ClickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Retry()
    {
        AudioManager.instance.PlayAudio(SFXType.ClickSound);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
