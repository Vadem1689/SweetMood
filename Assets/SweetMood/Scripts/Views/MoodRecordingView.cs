using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodRecordingView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField _descriptionEmotions;
    [SerializeField] private Button _save;
    [Header("References")]
    [SerializeField] private List<IconEmotion> _emotions;
    [SerializeField] private ProgressEmotionsView _progressEmotionsView;

    private void Start()
    {
        foreach (var emotion in _emotions)
            emotion.UpdateViewProgressEmotionEventHandler.AddListener(_progressEmotionsView.UpdateProgressEmotions);
    }
}
