using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Screen : MonoBehaviour
{
    public event Action<Screen, PuzzlePiece> OnPuzzleNeeded;

    [SerializeField] private PuzzlePiece _puzzlePiecePrefab;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private bool _active;
    public bool Active 
    { 
        get 
        {
            return _active;
        } 
        set 
        { 
            _active = value;
            if(value == false)
            {
                ChangeVisualsToCalm();
            }
            else
            {
                ChangeVisualsToAlert();            
            }
        }
    }

    public bool Interactable { private get; set; }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void ChangeVisualsToAlert()
    {
        _animator.SetBool("IsBroken", true);
    }

    private void ChangeVisualsToCalm()
    {
        _animator.SetBool("IsBroken", false);
    }

    private void OnMouseDown()
    {
        if (!Interactable)
            return;

        Debug.Log("OnMouse Down " + name);
        if (!_active)
            return;
        Debug.Log("OnMouse Down And Works " + name);
        OnPuzzleNeeded?.Invoke(this, _puzzlePiecePrefab);
    }
}
