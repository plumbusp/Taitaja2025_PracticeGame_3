using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Slider _HealthSlider;
    [SerializeField] private float _decreasingSpeed;
    [SerializeField] private float _increasingSpeed;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _deadValue;

    private int _activeDecreasersAmount;
    public int ActiveDecreasersAmount
    {
        get
        {
            return _activeDecreasersAmount;
        }
        set
        {
            _activeDecreasersAmount = value;
        }
    }

    //[SerializeField] private EndGameScreenController _endGameScreenController;

    private void Start()
    {
        _HealthSlider.maxValue = _maxValue;
        _HealthSlider.minValue = 0;
        _HealthSlider.value = _maxValue;

        //_table.OnPerformed += DecreaseValue;
    }
    private void Update()
    {
        if(_activeDecreasersAmount > 0)
            _HealthSlider.value -= _decreasingSpeed * _activeDecreasersAmount * Time.deltaTime;
        else
            _HealthSlider.value += _increasingSpeed * Time.deltaTime;

        if (_HealthSlider.value >= _deadValue)
        {
            // TO-DO DEAD
        }
    }
    private void DecreaseValue()
    {
        _HealthSlider.value = 0;
    }
}
