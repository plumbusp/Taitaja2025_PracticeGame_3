using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Screen : MonoBehaviour
{
    public event Action OnProblemFixed;

    [SerializeField] private Color _calmColor;
    [SerializeField] private Color _alertColor;

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
}
