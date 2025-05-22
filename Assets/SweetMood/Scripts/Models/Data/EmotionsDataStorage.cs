using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionsDataStorage", menuName = "SO/EmotionsDataStorage")]
public class EmotionsDataStorage : ScriptableObject
{
    [SerializeField] private List<EmotionsDataForDay> _emotionsForDays;

    public void AddEmotion(EmotionsDataForDay emotionsOfDay)
        => _emotionsForDays.Add(emotionsOfDay);
}
