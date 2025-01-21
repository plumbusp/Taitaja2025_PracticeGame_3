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
    [SerializeField] private int _SizeX; // Number of columns
    [SerializeField] private int _SizeY; // Number of rows
    [SerializeField] private GameObject _puzzleVisuals;
    [SerializeField] private float _PuzzleScale;

    private PuzzlePiece _curentPiecePrefab;
    private List<PuzzlePiece> _inScenePieces;
    private int _emptyLocation;
    private bool _shuffling = false;
    private bool _puzzleExists = false;
    private bool _puzzleCompleted = false;

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
        _inScenePieces = new List<PuzzlePiece>();
    }

    public void OpenPuzzle(PuzzlePiece piecePrefab)
    {
        _puzzleVisuals.SetActive(true);

        DestroyCurrentPuzzle();

        _curentPiecePrefab = piecePrefab;
        CreateGamePieces();
        Shuffle();
        _puzzleExists = true;
        _puzzleCompleted = false;
    }

    public void ClosePuzzle()
    {
        _puzzleVisuals.SetActive(false);
    }

    void Update()
    {
        if (!_puzzleExists || _puzzleCompleted) return;

        CheckCompletion();
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        int targetIndex = i + offset;
        if (targetIndex < 0 || targetIndex >= (_SizeX * _SizeY)) return false;

        // Ensure column constraints are met
        if ((i % _SizeX == 0 && offset == -1) || // Prevent wrap-around to the previous row
            ((i + 1) % _SizeX == 0 && offset == 1)) // Prevent wrap-around to the next row
        {
            return false;
        }

        if (targetIndex == _emptyLocation)
        {
            (_inScenePieces[i], _inScenePieces[targetIndex]) = (_inScenePieces[targetIndex], _inScenePieces[i]);
            (_inScenePieces[i].Transform.localPosition, _inScenePieces[targetIndex].Transform.localPosition) =
                (_inScenePieces[targetIndex].Transform.localPosition, _inScenePieces[i].Transform.localPosition);
            _emptyLocation = i;
            return true;
        }
        return false;
    }

    private void CheckCompletion()
    {
        for (int i = 0; i < _inScenePieces.Count; i++)
        {
            if (_inScenePieces[i].name != $"{i}") return; // If not complete
        }

        // Puzzle is complete
        foreach (var piece in _inScenePieces)
        {
            piece.OnClick -= HandlePieceChange;
        }

        Debug.Log("Puzzle Complete!");
        _puzzleCompleted = true;
        OnPuzzleComplete?.Invoke();
    }

    private void Shuffle()
    {
        _shuffling = true;
        int count = 0;
        while (count < (_SizeX * _SizeY * 3))
        {
            int rnd = UnityEngine.Random.Range(0, _SizeX * _SizeY);
            if (SwapIfValid(rnd, -_SizeX, _SizeX) ||
                SwapIfValid(rnd, +_SizeX, _SizeX) ||
                SwapIfValid(rnd, -1, 0) ||
                SwapIfValid(rnd, +1, _SizeX - 1))
            {
                count++;
            }
        }
        _shuffling = false;
    }

    private void CreateGamePieces()
    {
        _puzzleBoard.localScale = Vector3.one * _PuzzleScale; // Apply scale to the entire puzzle.

        float pieceWidth = 1f / _SizeX;
        float pieceHeight = 1f / _SizeY;

        for (int row = 0; row < _SizeY; row++)
        {
            for (int col = 0; col < _SizeX; col++)
            {
                PuzzlePiece piece = Instantiate(_curentPiecePrefab, _puzzleBoard);
                piece.OnClick += HandlePieceChange;

                _inScenePieces.Add(piece);

                piece.Transform.localPosition = new Vector3(-1 + (2 * pieceWidth * col) + pieceWidth,
                                                            +1 - (2 * pieceHeight * row) - pieceHeight,
                                                            0);
                piece.Transform.localScale = new Vector3((2 * pieceWidth) - _GapThickness,
                                                         (2 * pieceHeight) - _GapThickness,
                                                         1);
                piece.name = $"{(row * _SizeX) + col}";

                // Mark the bottom-right piece as empty
                if (row == _SizeY - 1 && col == _SizeX - 1)
                {
                    _emptyLocation = (_SizeY * _SizeX) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    // Set UV mapping for the piece
                    float gap = _GapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    uv[0] = new Vector2((pieceWidth * col) + gap, 1 - ((pieceHeight * (row + 1)) - gap));
                    uv[1] = new Vector2((pieceWidth * (col + 1)) - gap, 1 - ((pieceHeight * (row + 1)) - gap));
                    uv[2] = new Vector2((pieceWidth * col) + gap, 1 - ((pieceHeight * row) + gap));
                    uv[3] = new Vector2((pieceWidth * (col + 1)) - gap, 1 - ((pieceHeight * row) + gap));
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
        }
        _inScenePieces.Clear();
    }

    private void HandlePieceChange(PuzzlePiece piece)
    {
        Debug.Log("HandlePieceChange");
        for (int i = 0; i < _inScenePieces.Count; i++)
        {
            if (_inScenePieces[i] == piece)
            {
                Debug.Log("Yeap Yeap");
                // Check each direction to see if valid move.
                // We break out on success so we don't carry on and swap back again.
                if (SwapIfValid(i, -_SizeX, _SizeX)) { break; }
                if (SwapIfValid(i, +_SizeX, _SizeX)) { break; }
                if (SwapIfValid(i, -1, 0)) { break; }
                if (SwapIfValid(i, +1, _SizeX - 1)) { break; }
            }
        }
    }

}