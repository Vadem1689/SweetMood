using UnityEngine;

public class MoodRecordingService : BaseService
{
    private MoodRecordingController _moodRecordingController;

    [SerializeField] private MoodRecordingView _moodRecordingView;
    [SerializeField] private EmotionsDataStorage _emotionsDataStorage;

    public override void Initialize()
    {
        _moodRecordingController = new MoodRecordingController(_moodRecordingView, _emotionsDataStorage);
        _moodRecordingController.Initialize();
    }
}
