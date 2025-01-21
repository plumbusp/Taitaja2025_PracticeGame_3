using TMPro;
using UnityEngine;

public class WinStateTimer : MonoBehaviour
{
    [SerializeField] private float _secUntilWin = 10;
    [SerializeField] private TMP_Text _timeText;

    void Update()
    {
        if (_secUntilWin > 0)
        {
            _secUntilWin -= Time.deltaTime;
            DisplayTime(_secUntilWin);
            return;
        }
        DisplayTime(0);
        Debug.Log("Dead");
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
