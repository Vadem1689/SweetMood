using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ProgressEmotionsView : MonoBehaviour
{
    [SerializeField] private Slider _progressEmotions;
    [SerializeField] private TextMeshProUGUI _numberEmotions;

    public void UpdateProgressEmotions(int value)
        => _progressEmotions.value = value;

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
