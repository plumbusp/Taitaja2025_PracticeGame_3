using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Screen : MonoBehaviour
{
    public event Action<Screen, GameObject> OnPuzzleNeeded;

    [SerializeField] private Color _calmColor;
    [SerializeField] private Color _alertColor;
    [SerializeField] private GameObject _puzzlePiecePrefab;

    private SpriteRenderer _spriteRenderer;

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
    }

    private void ChangeVisualsToAlert()
    {
        _spriteRenderer.color = _alertColor;
    }

    private void ChangeVisualsToCalm()
    {
        _spriteRenderer.color = _calmColor;
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
