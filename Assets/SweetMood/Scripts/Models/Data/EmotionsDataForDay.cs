using System;
using UnityEngine;

[Serializable]
public class EmotionsDataForDay
{
    [SerializeField] private DateTime _date;
    [SerializeField] private int _emotionsProgress;
    [SerializeField] private string _descriptionEmotions;

    public EmotionsDataForDay(DateTime date, int emotionsProgress, string descriptionEmotions)
    {
        _date = date;
        _emotionsProgress = emotionsProgress;
        _descriptionEmotions = descriptionEmotions;
    }
}
