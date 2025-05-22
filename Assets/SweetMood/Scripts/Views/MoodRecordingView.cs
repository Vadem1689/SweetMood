using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class MoodRecordingView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField _descriptionEmotions;
    [SerializeField] private Button _save;
    [Header("References")]
    [SerializeField] private List<IconEmotion> _emotions;
    [SerializeField] private ProgressEmotionsView _progressEmotionsView;

    public ReactiveCommand<EmotionsDataForDay> SavingEmotionsCommad = new();

    private void Start()
    {
        UniRxService.AddObjectDisposable(SavingEmotionsCommad);

        _save.onClick.AddListener(() =>
        {
            var emotionsOfDay = new EmotionsDataForDay(DateTime.Now, _progressEmotionsView.Current, _descriptionEmotions.text);
            SavingEmotionsCommad.Execute(emotionsOfDay);
        });

        foreach (var emotion in _emotions)
            emotion.UpdateViewProgressEmotionEventHandler.AddListener(_progressEmotionsView.UpdateProgressEmotions);
    }

    private void OnDestroy()
    {
        _save.onClick.RemoveAllListeners();
        UniRxService.Dispose(SavingEmotionsCommad);
    }
}
