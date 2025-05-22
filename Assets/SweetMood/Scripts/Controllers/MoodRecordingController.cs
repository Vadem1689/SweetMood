using UniRx;

public class MoodRecordingController
{
    private MoodRecordingView _moodRecordingView;
    private EmotionsDataStorage _emotionsDataStorage;

    public MoodRecordingController(MoodRecordingView moodRecordingView, EmotionsDataStorage emotionsDataStorage)
    {
        _moodRecordingView = moodRecordingView;
        _emotionsDataStorage = emotionsDataStorage;
    }

    public void Initialize()
    {
        _moodRecordingView.SavingEmotionsCommad.Subscribe(emotionsOfDay =>
        {
            _emotionsDataStorage.AddEmotion(emotionsOfDay);
        });
    }
}
