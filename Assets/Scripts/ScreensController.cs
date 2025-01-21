using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensController : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    private float _timer = 0;

    public List<Screen> _screens;
    private List<Screen> _activeScreens = new();
    private Screen _openedScreen;
    private bool _busy = false;
    private bool _inPuzzleWindow;

    private void Start()
    {
        foreach (Screen screen in _screens)
        {
            screen.OnPuzzleNeeded += OpenNewPuzzle;
        }
    }
    private void OnDestroy()
    {
        foreach (Screen screen in _screens)
        {
            screen.OnPuzzleNeeded -= OpenNewPuzzle;
        }
    }

    private void Update()
    {
        if (_inPuzzleWindow)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseCurrentPuzzle();
            }
        }

        if (_busy)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            _timer = 0;
            if (_activeScreens.Count >= _screens.Count)
                return;
            StartCoroutine(HandleProblemActivation());
        }
    }

    private IEnumerator HandleProblemActivation()
    {
        _busy = true;
        while (true)
        {
            if (ActivateRandomProblem())
            {
                _busy = false;
                yield break;
            }
        }
    }

    private bool ActivateRandomProblem()
    {
        int randomIndex = Random.Range(0, _screens.Count);
        if (_activeScreens.Contains(_screens[randomIndex]))
            return false;

        var problem = _screens[randomIndex];
        _activeScreens.Add(problem);
        problem.Active = true;

        return true;
    }

    private void OpenNewPuzzle(Screen correspondingScreen, GameObject puzzlePrefab)
    {
        foreach (var screen in _screens)
        {
            screen.Interactable = false;
        }
        _openedScreen = correspondingScreen;
        PuzzleManager.Instance.OpenPuzzle(puzzlePrefab);
        _inPuzzleWindow = true;
    }

    private void CloseCurrentPuzzle()
    {
        foreach (var screen in _screens)
        {
            screen.Interactable = true;
        }
        PuzzleManager.Instance.ClosePuzzle();
        _inPuzzleWindow = false;
    }
}
