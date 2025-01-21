using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public event Action OnPuzzleComplete;

    [SerializeField] private Transform _puzzleBoard;
    [SerializeField] private float _GapThickness;
    [SerializeField] private int _Size;
    [SerializeField] private GameObject _puzzleVisuals;
    [SerializeField] private float _PuzzleScale;

    private GameObject _curentPiecePrefab;
    private List<Transform> _inScenePieces;
    private int _emptyLocation;
    private bool _shuffling = false;
    private bool _puzzleExists;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _puzzleExists = false;
        _inScenePieces = new List<Transform>();
    }

    public void OpenPuzzle(GameObject piecePrefab)
    {
        _puzzleVisuals.SetActive(true);

        DestroyCurrentPuzzle();

        _curentPiecePrefab = piecePrefab;
        CreateGamePieces();
        Shuffle();
        _puzzleExists = true;
    }
    public void ClosePuzzle()
    {
        _puzzleVisuals.SetActive(false);
    }

    void Update()
    {
        //if(_shuffling)
        //    return;

        if (!_puzzleExists)
            return;

        if (CheckCompletion())
        {
            Debug.Log("CheckCompletion");
            OnPuzzleComplete?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < _inScenePieces.Count; i++)
                {
                    if (_inScenePieces[i] == hit.transform)
                    {
                        // Check each direction to see if valid move.
                        // We break out on success so we don't carry on and swap back again.
                        if (SwapIfValid(i, -_Size, _Size)) { break; }
                        if (SwapIfValid(i, +_Size, _Size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, _Size - 1)) { break; }
                    }
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % _Size) != colCheck) && ((i + offset) == _emptyLocation))
        {
            (_inScenePieces[i], _inScenePieces[i + offset]) = (_inScenePieces[i + offset], _inScenePieces[i]);
            (_inScenePieces[i].localPosition, _inScenePieces[i + offset].localPosition) = ((_inScenePieces[i + offset].localPosition, _inScenePieces[i].localPosition));
            _emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < _inScenePieces.Count; i++)
        {
            if (_inScenePieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    private void Shuffle()
    {
        _shuffling = true;
        int count = 0;
        int last = 0;
        while (count < (_Size * _Size * _Size))
        {
            int rnd = UnityEngine.Random.Range(0, _Size * _Size);
            if (rnd == last) { continue; }
            last = _emptyLocation;

            if (SwapIfValid(rnd, -_Size, _Size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +_Size, _Size))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, _Size - 1))
            {
                count++;
            }
        }
        _shuffling = false;
    }

    // Create the game setup with size x size pieces.
    private void CreateGamePieces()
    {
        _puzzleBoard.localScale = Vector3.one * _PuzzleScale; // Apply scale to the entire puzzle.

        float width = 1 / (float)_Size;
        for (int row = 0; row < _Size; row++)
        {
            for (int col = 0; col < _Size; col++)
            {
                Transform piece = Instantiate(_curentPiecePrefab.transform, _puzzleBoard);
                _inScenePieces.Add(piece);

                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) - width,
                                                  0);
                piece.localScale = ((2 * width) - _GapThickness) * Vector3.one;
                piece.name = $"{(row * _Size) + col}";

                if ((row == _Size - 1) && (col == _Size - 1))
                {
                    _emptyLocation = (_Size * _Size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = _GapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    mesh.uv = uv;
                }
            }
        }
    }
    private void DestroyCurrentPuzzle()
    {
        for (int i = _inScenePieces.Count - 1; i >= 0; i--)
        {
            Destroy(_inScenePieces[i].gameObject);
            Debug.Log("Destroyed " + name);
            _inScenePieces[i] = null;
        }
        _inScenePieces.Clear();
    }
}
