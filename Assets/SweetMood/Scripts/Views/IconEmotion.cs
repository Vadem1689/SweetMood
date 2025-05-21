using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IconEmotion : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _progressValueEmotion;
    [Header("UI")]
    [SerializeField] private Button _buttonEmotion;

    [HideInInspector]
    public UnityEvent<int> UpdateViewProgressEmotionEventHandler;

    private void Start()
    {
        _buttonEmotion.onClick.AddListener(UpdateViewProgressEmotion);
    }

    private void UpdateViewProgressEmotion()
    {
        UpdateViewProgressEmotionEventHandler?.Invoke(_progressValueEmotion);
    }

    private void OnValidate()
    {
        if (_buttonEmotion == null)
            _buttonEmotion = GetComponent<Button>();
    }

    private void OnDestroy()
    {
        UpdateViewProgressEmotionEventHandler.RemoveAllListeners();
        _buttonEmotion.onClick.RemoveAllListeners();
    }
}
