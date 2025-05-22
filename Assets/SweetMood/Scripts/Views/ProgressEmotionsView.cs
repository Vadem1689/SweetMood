using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using TMPro;
using DG.Tweening;

public class ProgressEmotionsView : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _durationChangingProgressionEmotions;
    [Header("UI")]
    [SerializeField] private Slider _progressEmotions;
    [SerializeField] private TextMeshProUGUI _numberEmotions;

    public void UpdateProgressEmotions(int value)
        => DOTween.To(value => _progressEmotions.value = value, _progressEmotions.value, value, _durationChangingProgressionEmotions);

    private void Start()
    {
        _progressEmotions.onValueChanged.AddListener(value => { OnValueChanged(value); });
    }

    private void OnValueChanged(float value)
    {
        var valueInt = Mathf.FloorToInt(value);
        _numberEmotions.text = valueInt.ToString();
    }

    private void OnDestroy()
    {
        _progressEmotions.onValueChanged.RemoveAllListeners();
    }
}
