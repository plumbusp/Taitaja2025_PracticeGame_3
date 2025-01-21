using System;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class PuzzlePiece : MonoBehaviour
{
    public event Action<PuzzlePiece> OnClick;
    private Transform _transform;   
    public Transform Transform
    {
        get
        { return _transform; }
        private set { _transform = value; }
    }
    private void Awake()
    {
        _transform = transform;
    }
    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }
}
